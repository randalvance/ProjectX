using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Business;
using ProjectX.Models;
using ProjectX.ViewModels.Account;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ProjectX.Core;

namespace ProjectX.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = new BankAccount()
                {
                    AccountName = model.AccountName,
                    AccountNumber = model.AccountNumber,
                    Password = HashPassword(model.Password)
                };

                try
                {
                    _accountService.CreateAccount(account);
                }
                catch (AccountCreationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }

                await SignIn(account);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var account = _accountService.RetrieveByAccountNumber(model.AccountNumber);

                var passwordHash = HashPassword(model.Password);

                if (account == null || account.Password != passwordHash)
                {
                    ModelState.AddModelError("", "Invalid Account Number or Password");

                    return View(model);
                }

                await SignIn(account);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(Constants.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Deposit()
        {
            var account = GetCurrentUserBankAccount();

            var viewModel = new TransactionViewModel
            {
                Type = TransactionViewModel.TransactionType.Deposit,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };

            return View("Transaction", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(TransactionViewModel transaction)
        {
            if (ModelState.IsValid)
            {
                var account = GetCurrentUserBankAccount();

                try
                {
                    _accountService.Deposit(account, transaction.Amount);

                    return RedirectToAction(nameof(AccountController.TransactionSuccess), new { type = TransactionViewModel.TransactionType.Deposit, amount = transaction.Amount });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "The data was updated by another user, please retry the transaction.");
                }
            }

            return View("Transaction", transaction);
        }

        [HttpGet]
        public IActionResult Widthraw()
        {
            var account = GetCurrentUserBankAccount();

            var viewModel = new TransactionViewModel
            {
                Type = TransactionViewModel.TransactionType.Widthraw,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };

            return View("Transaction", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Widthraw(TransactionViewModel transaction)
        {
            if (ModelState.IsValid)
            {
                var account = GetCurrentUserBankAccount();

                try
                {
                    _accountService.Widthraw(account, transaction.Amount);

                    return RedirectToAction(nameof(AccountController.TransactionSuccess), new { type = TransactionViewModel.TransactionType.Widthraw, amount = transaction.Amount });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "The data was updated by another user, please retry the transaction.");
                }
                catch (InsufficientFundsException)
                {
                    ModelState.AddModelError("", "You have insufficient funds.");
                }
            }

            return View("Transaction", transaction);
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            var account = GetCurrentUserBankAccount();

            var viewModel = new TransferViewModel
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(TransferViewModel transaction)
        {
            if (ModelState.IsValid)
            {
                var sourceAccount = GetCurrentUserBankAccount();
                var destinationAccount = _accountService.RetrieveByAccountNumber(transaction.DestinationAccountNumber);

                if (destinationAccount == null)
                {
                    ModelState.AddModelError(nameof(TransferViewModel.DestinationAccountNumber), "Target account does not exist.");
                    return View(transaction);
                }

                try
                {
                    _accountService.Transfer(sourceAccount, destinationAccount, transaction.Amount);

                    return RedirectToAction(nameof(AccountController.TransactionSuccess), 
                        new { type = TransactionViewModel.TransactionType.Transfer, amount = transaction.Amount, destAcct = transaction.DestinationAccountNumber });
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "The data was updated by another user, please retry the transaction.");
                }
                catch (InsufficientFundsException)
                {
                    ModelState.AddModelError("", "You have insufficient funds.");
                }
                catch (InvalidTransferException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            return View(transaction);
        }

        [HttpGet]
        public IActionResult TransactionSuccess(TransactionViewModel.TransactionType type, decimal amount, string destAcct = "")
        {
            if (type == TransactionViewModel.TransactionType.Transfer)
            {
                return View(new TransferViewModel
                {
                    DestinationAccountNumber = destAcct,
                    Type = type,
                    Amount = amount
                });
            }

            return View(new TransactionViewModel
            {
                Type = type,
                Amount = amount
            });
        }

        [HttpGet]
        public IActionResult ViewTransactions()
        {
            var accountWithTransactions = GetCurrentUserBankAccount(true);

            return View(new ViewTransactionsViewModel
            {
                Transactions = accountWithTransactions.Transactions
            });
        }


        private string HashPassword(string password)
        {
            // In real world, hash the password
            return password;
        }

        private async Task SignIn(BankAccount account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.AccountName)
                };

            var userIdentity = new ClaimsIdentity(claims, "login");

            var principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync(Constants.AuthenticationScheme, principal);
        }

        private BankAccount GetCurrentUserBankAccount(bool includeTransactions = false)
        {
            var accountName = User.Identity.Name;

            return _accountService.RetrieveByAccountName(accountName, includeTransactions);
        }
    }
}

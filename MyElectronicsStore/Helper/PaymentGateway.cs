using MyElectronicsStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;

namespace MyElectronicsStore.Helper
{
    public class PaymentGateway
    {
        //Note: Replace YourDbContext with actual name of your database context
        private ApplicationDbContext db = new ApplicationDbContext();
        //Method to handle authorization and payment operation
        public bool Pay(string token, double totalAmount, IEnumerable<Cart>
        carts)
        {
            try
            {
                //check if user is logged in
                if (HttpContext.Current.User.Identity.IsAuthenticated == true)
                {
                    //Get user
                    var user = HttpContext.Current.User.Identity.Name;
                    //Get token info from payment token table
                    var paymentToken = db.PaymentTokens.Where(x => x.UserName
                    == user && x.Token == token).FirstOrDefault();
                    //If user does not have payment token or invalid, then
                    //token info will be null
                    if (paymentToken == null)
                    {
                        return false;
                    }
                    else
                    {
                        //Check if available amount is sufficient for payment
                        if (paymentToken.Amount < totalAmount)
                        {
                            return false;
                        }
                        //User is valid and has payment token with sufficient
                        //amount.Proceed forward for payment
                        //Note: If System.Transactions is not present, right
                        //click references folder > add reference > search for System.Transactions
                        //and add it
                        using (TransactionScope scope = new TransactionScope())
                        {
                            //deduct amount
                            paymentToken.Amount -= totalAmount;
                            db.Entry(paymentToken).State =
                            EntityState.Modified;
                            db.SaveChanges();
                            //maintain invoice
                            MaintainInvoice(totalAmount, carts);
                            //If both processes of amount deduction and
                            //maintain invoice are successful, commit the transaction. Otherwise it is
                            //automatically rolled back by system.transactions
                            scope.Complete();
                        }
                        //if we return here, everything is good, return true
                        return true;
                    }
                }
                else
                {
                    //return false if user is not logged in
                    return false;
                }
            }
            catch (Exception ex)
            {
                //return false if exception is caught
                return false;
            }
        }

        //Method to maintain invoice. This also helps in maintaining history of
        //customer purchases
private void MaintainInvoice(double totalAmount, IEnumerable<Cart>
carts)
        {
            Invoice invoice = new Invoice();
            invoice.PaymentDate = DateTime.Now;
            invoice.TotalAmount = totalAmount;
            invoice.UserName = HttpContext.Current.User.Identity.Name;
            //add invoice to database.
            db.Invoices.Add(invoice);
            db.SaveChanges();
            //add invoice details from cart
            foreach (var cart in carts)
            {
                InvoiceDetail detail = new InvoiceDetail();
                detail.InvoiceId = invoice.Id;
                detail.ProductId = cart.ProductId;
                detail.Quantity = cart.Quantity;
                detail.SubAmount = Convert.ToDouble(cart.Price *
                cart.Quantity);
                db.InvoiceDetails.Add(detail);
            }
            //save changes to database
            db.SaveChanges();
        }


    }
}
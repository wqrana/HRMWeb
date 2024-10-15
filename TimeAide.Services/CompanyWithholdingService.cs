using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class CompanyWithholdingService
    {
        public static void UpdateCompensationTransaction(int id, string selectedTransactionIds)
        {
            TimeAideContext db = new TimeAideContext();
            var selectedTransactionsList = selectedTransactionIds.Split(',').ToList();
            List<CompensationTransaction> transactionAddList = new List<CompensationTransaction>();
            List<CompensationTransaction> transactionRemoveList = new List<CompensationTransaction>();
            var existingTransactionList = db.CompensationTransaction.Where(w => w.CompanyCompensationId == id).ToList();

            foreach (var transactionItem in existingTransactionList)
            {
                var RecCnt = selectedTransactionsList.Where(w => w == transactionItem.TransactionConfigurationId.ToString()).Count();
                if (RecCnt == 0)
                {
                    transactionRemoveList.Add(transactionItem);
                }

            }
            foreach (var selectedTransactionId in selectedTransactionsList)
            {
                if (selectedTransactionId == "") continue;
                int transactionId = int.Parse(selectedTransactionId);
                var recExists = existingTransactionList.Where(w => w.TransactionConfigurationId == transactionId).Count();
                if (recExists == 0)
                {
                    transactionAddList.Add(new CompensationTransaction() { CompanyCompensationId = id, TransactionConfigurationId = transactionId });

                }
            }

            db.CompensationTransaction.RemoveRange(transactionRemoveList);
            db.CompensationTransaction.AddRange(transactionAddList);

            db.SaveChanges();
        }
    }
}

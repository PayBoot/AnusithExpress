using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class ConsolidationService : IConsolidation
    {
        private ConsolidateUtility _consolidate = new ConsolidateUtility();
        private ItemUtility _item = new ItemUtility();
        public ConsolidationModel GetConsolidationDetailByConsolidationId(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var sourceDetail = _consolidate.GetSingle(db, consolidationId);
                var sourceItems = _consolidate.GetAllItems(db).Where(c => c.Consolidator.Id == consolidationId).ToList();
                ConsolidationModel result = new ConsolidationModel
                {
                    Detail = _consolidate.AssignSingleConsolidationList(sourceDetail),
                    Items = _consolidate.AssignConsolidationItemsList(sourceItems)
                };
                return result;

            }
        }

        public List<ConsolidationListModel> GetConsolidationListByCustomerId(int customerId, DateTime? searchFrom, DateTime? searchTo)
        {
            using (var db = new EntityContext())
            {
                var source = _consolidate.GetAll(db).Where(c => c.Customer.Id == customerId).ToList();
                if (searchFrom != null)
                {
                    source = source.Where(c => c.ConsolidatedDate >= searchFrom).ToList();
                }
                if (searchTo != null)
                {
                    source = source.Where(c => c.ConsolidatedDate <= searchTo).ToList();
                }
                List<ConsolidationListModel> result = _consolidate.AssignConsolidationList(source);
                return result;
            }
        }


    }
}

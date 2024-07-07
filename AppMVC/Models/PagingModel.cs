using System;
using System.Collections;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Policy;

namespace AppMVC.Models
{
    public class PagingModel
    {
        public PagingModel(int currentPage, int totalItem, int itemPerPage, string urlPage)
        {
            // Set Total Item First
            TotalItem = totalItem;

            AdjustPageParams(ref currentPage, ref itemPerPage);
            CurrentPage = currentPage;
            ITEM_PER_PAGE = itemPerPage;
            GenerateUrl = (p) => urlPage + @$"?p={p}&size={ITEM_PER_PAGE}";
        }

        // Pass
        public int TotalItem { get; set; }
        public int ITEM_PER_PAGE { get; set; }
        public int CurrentPage { get; set; }
        public Func<int?, string> GenerateUrl { get; set; }

        // Caculate
        public int CountPages { get; set; }

        private void AdjustPageParams(ref int currentPage, ref int itemPerPage)
        {
            // Valid ItemPerPage
            if (itemPerPage <= 0)
            {
                itemPerPage = 10;
            }

            // Cacular Count Page
            CountPages = (int)Math.Ceiling((double)TotalItem / itemPerPage);
            
            // Vaild Current Page
            if (currentPage <= 0)
            {
                currentPage = 1;
            }

            if (currentPage > CountPages)
            {
                currentPage = CountPages;
            }
        }

        // Take item per page by current page
        public List<T> TakePagingItem<T>(List<T> items)
        {
            return items.Skip(ITEM_PER_PAGE * (CurrentPage - 1)).Take(ITEM_PER_PAGE).ToList();
        }
        
    }
   
}
using Sakura.AspNet.Mvc.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projectSchedule1.ViewModels
{
    public class PagedListOption
    {
        public static PagerOptions getPagedListOption()
        {
            var pagerOptions = new PagerOptions
            {
                ExpandPageLinksForCurrentPage = 2, // Will display more 2 pager buttons before and after current page.
            
                Layout = PagedListPagerLayouts.NoFirstAndLastButton, // Layout controls which elements will be displayed in the pager. For more information, please read the documentation.

                // Options for all pager items.
                Items = new PagerItemOptions
                {
                    TextFormat = "{0}", // The format for the pager button text, here means the content is just the actual page number. This property is used with string.Format method.
                    LinkParameterName = "page", // This property means the generated pager button url will append the "page={pageNumber}" to the current URL.
                },

                // Configure for "go to next" button
                NextButton = new SpecialPagerItemOptions
                {
                    Text = "Next",
                    InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable, // When there is no next page, disable this button
                    LinkParameterName = "page"
                },

                // Configure for "go to previous" button
                PreviousButton = new SpecialPagerItemOptions
                {
                    Text = "Previous",
                    InactiveBehavior = SpecialPagerItemInactiveBehavior.Disable, // When there is no next page, disable this button
                    LinkParameterName = "page"
                },

                // Configure for omitted buttons (placeholder button when there's too many pages)
                Omitted = new PagerItemOptions
                {
                    Text = "...",
                    Link = string.Empty // disable link
                },
            };
            return pagerOptions;
        }
    }
}

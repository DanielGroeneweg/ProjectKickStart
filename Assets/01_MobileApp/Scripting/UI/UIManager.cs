using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [Header("Page Management")]
    [Tooltip("The pages (panels) managed by this UI Manager")]
    public List<UIPage> pages;

    /// <summary>
    /// Goes to a page by that page's index.
    /// </summary>
    /// <param name="pageIndex">The index in the page list to go to</param>
    public void GoToPage(int pageIndex)
    {
        if (pageIndex < pages.Count && pages[pageIndex] != null)
        {
            SetActiveAllPages(false);
            pages[pageIndex].gameObject.SetActive(true);
            pages[pageIndex].SetSelectedUIToDefault();
        }
    }

    /// <summary>
    /// Goes to a page by that page's index.
    /// </summary>
    /// <param name="pageIndex">The index in the page list to go to</param>
    public void GoToPage(UIPage page)
    {
        int pageIndex = pages.IndexOf(page);

        try
        {
            if (pageIndex < pages.Count && pages[pageIndex] != null)
            {
                SetActiveAllPages(false);
                pages[pageIndex].gameObject.SetActive(true);
                pages[pageIndex].SetSelectedUIToDefault();
            }
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            Debug.LogError($"Error while trying to go to page {page.name} with index {pageIndex}: {e.Message}");
        }
    }

    public void GoToOriginPage(UIPage currentPage)
    {
        UIPage toGoToPage = currentPage.OriginalCallerPage;
        if (toGoToPage == null)
            return;

        int index = pages.IndexOf(toGoToPage);
        Debug.Log($"TF IS GOING ON???? {index}");
        GoToPage(index);
    }

    /// <summary>
    /// Turns all stored pages on or off depending on the passed parameter.
    /// </summary>
    /// <param name="activeState">The state to set all pages to, true to active them all, false to deactivate them all</param>
    public void SetActiveAllPages(bool activeState)
    {
        if (pages == null)
        {
            return;
        }
        foreach (UIPage page in pages)
        {
            if (page != null)
            {
                page.gameObject.SetActive(activeState);
            }
        }
    }
}
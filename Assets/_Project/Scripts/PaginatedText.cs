using TMPro;
using UnityEngine;

public class PaginatedText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI pageContent;

    [SerializeField]
    TextMeshProUGUI pageCounter;

    int currentPage = 1;

    int totalPages = 0;

    private void OnEnable()
    {
        currentPage = 1;
        pageContent.pageToDisplay = 1;
        pageCounter.text = $"{currentPage}/{pageContent.textInfo.pageCount}";
    }

    void Update()
    {
        if (totalPages == 0)
        {
            totalPages = pageContent.textInfo.pageCount;
            pageCounter.text = $"{currentPage}/{totalPages}";
        }
    }

    public void NextPage()
    {
        if (currentPage + 1 <= pageContent.textInfo.pageCount)
        {
            currentPage++;
            pageContent.pageToDisplay = currentPage;
        }

        pageCounter.text = $"{currentPage}/{pageContent.textInfo.pageCount}";
    }

    public void PreviousPage()
    {
        if (currentPage - 1 > 0)
        {
            currentPage--;
            pageContent.pageToDisplay = currentPage;
        }

        pageCounter.text = $"{currentPage}/{pageContent.textInfo.pageCount}";
    }
}

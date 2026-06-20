using SimulasiCPNS.Models;
using SimulasiCPNS.Services;
using SimulasiCPNS.Enums;

namespace SimulasiCPNS.Views;

public partial class QuestionPage : ContentPage, IQueryAttributable
{
    private readonly QuestionService _questionService;
    private readonly BookmarkedService _bookmarkedService;
    public string Mode { get; private set; }
    public CategoryDisplayItem Category { get; private set; }

    List<SubCategoryDisplayItem> _subCategories = [];
    List<Question> _questions = [];

    public QuestionPage(QuestionService questionService, BookmarkedService bookmarkedService)
    {
        InitializeComponent();
        _questionService = questionService;
        _bookmarkedService = bookmarkedService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("mode", out var mode)) Mode = mode?.ToString();

        if (query.TryGetValue("category", out var category)) Category = (CategoryDisplayItem)category;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        TitlePage.Text = Category.Category;
        DescriptionPage.Text = Category.Description;

        if (Category.Category == "Mixed")
        {
            _questions = await _questionService.GetQuestionsAsync();
        } else
        {
            _questions = await _questionService.GetQuestionsByCategoryAsync(Category.Category);
            _subCategories = await _questionService.GetSubCategoryByCategoryAsync(Category.Category);
        }

        if (Category.Category != "Mixed")
        {
            _subCategories = _subCategories.Prepend(new SubCategoryDisplayItem
            {
                SubCategory = "Semua",
                IsSelected = true
            }).ToList();
        }
        
        SubCategoryListLayout.BindingContext = _subCategories;

        _questions = AddNumberOnQuestion(_questions);

        QuestionListLayout.BindingContext = _questions;
    }

    private async void OnSubCategoryTapped(object sender, TappedEventArgs e)
    {
        var tappedItem = (SubCategoryDisplayItem)((BindableObject)sender).BindingContext;
        foreach (var item in _subCategories) item.IsSelected = false;

        tappedItem.IsSelected = true;

        if (sender is Border border && border.BindingContext is SubCategoryDisplayItem subCategory)
        {
            List<Question> questions = [];
            if (Category.Category == "Mixed")
            {
                questions = await _questionService.GetQuestionsAsync();
            }

            if (Category.Category != "Mixed" && subCategory.SubCategory == "Semua")
            {
                questions = await _questionService.GetQuestionsByCategoryAsync(Category.Category);
            }
            else
            {
                questions = await _questionService.GetQuestionsBySubCategoryAsync(Category.Category, subCategory.SubCategory);
            }

            questions = AddNumberOnQuestion(questions);

            QuestionListLayout.BindingContext = questions;
        }
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }

    private async void OnBookmarkTapped(object sender, TappedEventArgs e)
    {
        var label = sender as Label;
        var tappedItem = label?.BindingContext as Question;

        var isExist = await _questionService.GetQuestionById(tappedItem.Id);
        if (isExist is null) return;

        var isBookmarkExist = await _bookmarkedService.GetByQuestionIdAsync(isExist.Id);
        if (isBookmarkExist is null)
        {
            await _bookmarkedService.Insert(isExist.Id, BookmarkedQuestionType.Bookmark);
            label?.Text = "marked";
            return;
        }

        if(isBookmarkExist.Type != BookmarkedQuestionType.Bookmark.ToString())
        {
            await _bookmarkedService.Insert(isExist.Id, BookmarkedQuestionType.Bookmark);
            label?.Text = "marked";
            return;
        }

        await _bookmarkedService.Delete(isBookmarkExist);
        label?.Text = "☆";
    }

    private List<Question> AddNumberOnQuestion(List<Question> questions)
    {
        for (int i = 0; i < questions.Count; i++)
        {
            questions[i].DisplayNumber = i + 1;
        }

        return questions;
    }
}
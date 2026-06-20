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
    HashSet<int> _bookmarkedIds = [];

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

        await LoadBookmarkStatesAsync();

        QuestionListLayout.BindingContext = _questions;
    }

    private async Task LoadBookmarkStatesAsync()
    {
        var bookmarks = await _bookmarkedService.GetBookmarkedAsync();
        _bookmarkedIds = bookmarks
            .Where(b => b.Type == BookmarkedQuestionType.Bookmark.ToString())
            .Select(b => b.QuestionId)
            .ToHashSet();

        foreach (var question in _questions)
        {
            question.IsBookmarked = _bookmarkedIds.Contains(question.Id);
        }
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

            foreach (var q in questions)
                q.IsBookmarked = _bookmarkedIds.Contains(q.Id);

            QuestionListLayout.BindingContext = questions;
        }
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }

    private async void OnBookmarkTapped(object sender, TappedEventArgs e)
    {
        if (sender is not VisualElement element) return;
        if (element.BindingContext is not Question question) return;

        if (question.IsBookmarked)
        {
            var existing = await _bookmarkedService.GetByQuestionIdAsync(question.Id);
            if (existing is not null)
                await _bookmarkedService.Delete(existing);
            question.IsBookmarked = false;
            _bookmarkedIds.Remove(question.Id);
        }
        else
        {
            await _bookmarkedService.Insert(question.Id, BookmarkedQuestionType.Bookmark);
            question.IsBookmarked = true;
            _bookmarkedIds.Add(question.Id);
        }
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
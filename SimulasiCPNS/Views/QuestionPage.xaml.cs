using SimulasiCPNS.Models;
using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class QuestionPage : ContentPage, IQueryAttributable
{
    private readonly QuestionService _questionService;
    public string Mode { get; private set; }
    public CategoryDisplayItem Category { get; private set; }

    List<SubCategoryDisplayItem> _subCategories = [];
    List<Question> _questions = [];

    public QuestionPage(QuestionService questionService)
    {
        InitializeComponent();
        _questionService = questionService;
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

            for (int i = 0; i < questions.Count; i++)
            {
                questions[i].DisplayNumber = i + 1;
            }

            QuestionListLayout.BindingContext = questions;
        }
    }

    private async void OnBackTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
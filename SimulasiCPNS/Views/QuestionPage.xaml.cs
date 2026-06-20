using SimulasiCPNS.Models;
using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class QuestionPage : ContentPage, IQueryAttributable
{
    private readonly QuestionService _questionService;
    public string Mode { get; private set; }
    public CategoryDisplayItem Category { get; private set; }

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

        List<SubCategoryDisplayItem> subCategories = [];
        List<Question> questions = [];

        if (Category.Category == "Mixed")
        {
            questions = await _questionService.GetQuestionsAsync();
        } else
        {
            questions = await _questionService.GetQuestionsByCategoryAsync(Category.Category);
            subCategories = await _questionService.GetSubCategoryByCategoryAsync(Category.Category);
        }

        var subCategoriesList = subCategories.Select(c => c.SubCategory);
        if (Category.Category != "Mixed") subCategoriesList = subCategoriesList.Prepend("Semua");
        SubCategoryListLayout.BindingContext = subCategoriesList;
        
        QuestionListLayout.BindingContext = questions;
    }

    private async void OnSubCategoryTapped(object sender, TappedEventArgs e)
    {
        if (sender is Border border &&
            border.BindingContext is string subCategory)
        {
            List<Question> questions = [];
            if (Category.Category == "Mixed")
            {
                questions = await _questionService.GetQuestionsAsync();
                QuestionListLayout.BindingContext = questions;
                return;
            } 
            if (Category.Category != "Mixed" && subCategory == "Semua")
            {
                questions = await _questionService.GetQuestionsByCategoryAsync(Category.Category);
            }
            else
            {
                questions = await _questionService.GetQuestionsBySubCategoryAsync(Category.Category, subCategory);
            }

            QuestionListLayout.BindingContext = questions;
        }
    }
}
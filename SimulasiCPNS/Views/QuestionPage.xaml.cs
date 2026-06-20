using SimulasiCPNS.Models;
using SimulasiCPNS.Services;

namespace SimulasiCPNS.Views;

public partial class QuestionPage : ContentPage, IQueryAttributable
{
    private readonly QuestionService _questionService;
    public string Mode { get; private set; }
    public CategoryDisplayItem Category { get; private set; }

    public List<Question> _questions = [];

    public QuestionPage(QuestionService questionService)
    {
        InitializeComponent();
        _questionService = questionService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("mode", out var mode))
            Mode = mode?.ToString();

        if (query.TryGetValue("category", out var category))
            Category = (CategoryDisplayItem)category;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var categories = await _questionService.GetCategoriesAsync();
        var subCategories = categories.Where(q => !string.IsNullOrWhiteSpace(q.SubCategory)).Select(c => c.SubCategory);
        subCategories = subCategories.Prepend("Semua");
        SubCategoryListLayout.BindingContext = subCategories;

        var questions = await _questionService.GetQuestionsAsync();
        QuestionListLayout.BindingContext = questions;

    }
}
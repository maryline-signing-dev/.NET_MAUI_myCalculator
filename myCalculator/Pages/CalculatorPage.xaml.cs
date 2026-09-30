using System;
using System.Linq;
using myCalculator.Services;

namespace myCalculator.Pages;

public partial class CalculatorPage : ContentPage
{
    // ═════════════════════ Réglages d'affichage ═════════════════════

    private const double MaxContentWidth = 480;
    private const double PagePadding = 16;
    private const double BorderPadding = 16;
    private const double KeySpacing = 10;

    private double _resultMaxWidth = 280;
    private double _resultMaxFont = 56;

    // Moteur de calcul : contient toute la logique de la calculatrice.
    private readonly CalculatorEngine _engine = new();


    // ═════════════════════ Initialisation ═════════════════════

    public CalculatorPage()
    {
        InitializeComponent();

        // SizeChanged est déclenché au premier affichage et à chaque rotation / redimensionnement.
        SizeChanged += (_, _) => AdaptLayout();

        AngleModeLabel.Text = _engine.AngleModeText;
        AngleModeButton.Text = _engine.AngleModeText;

        RefreshDisplay();
    }


    // ═════════════════════ Gestionnaires d'événements ═════════════════════

    // ───────────── Touches numériques ─────────────

    private void OnDigitClicked(object? sender, EventArgs e)
    {
        if (sender is Button button && button.Text.Length == 1)
        {
            _engine.InputDigit(button.Text[0]);
            RefreshDisplay();
        }
    }


    // ───────────── Opérateurs ─────────────

    private void OnOperatorClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            _engine.SetOperator(button.Text);
            RefreshDisplay();
        }
    }


    // ───────────── Nombre décimal ─────────────

    private void OnDecimalClicked(object? sender, EventArgs e)
    {
        _engine.InputDecimal();
        RefreshDisplay();
    }


    // ───────────── Changement de signe ─────────────

    private void OnSignClicked(object? sender, EventArgs e)
    {
        _engine.ToggleSign();
        RefreshDisplay();
    }


    // ───────────── Effacer le dernier caractère ─────────────

    private void OnBackspaceClicked(object? sender, EventArgs e)
    {
        _engine.Backspace();
        RefreshDisplay();
    }


    // ───────────── Pourcentage ─────────────

    private void OnPercentClicked(object? sender, EventArgs e)
    {
        _engine.Percent();
        RefreshDisplay();
    }


    // ───────────── Tout effacer ─────────────

    private void OnClearClicked(object? sender, EventArgs e)
    {
        _engine.Clear();
        RefreshDisplay();
    }


    // ───────────── Égal ─────────────

    private void OnEqualsClicked(object? sender, EventArgs e)
    {
        _engine.Calculate();
        RefreshDisplay();
    }

    // ═════════════════════ Fonctions scientifiques ═════════════════════

    // ───────────── Carré ─────────────

    private void OnSquareClicked(object? sender, EventArgs e)
    {
        _engine.Square();
        RefreshDisplay();
    }


    // ───────────── Racine carrée ─────────────

    private void OnSquareRootClicked(object? sender, EventArgs e)
    {
        _engine.SquareRoot();
        RefreshDisplay();
    }


    // ───────────── Puissance ─────────────

    private void OnPowerClicked(object? sender, EventArgs e)
    {
        _engine.SetPowerOperator();
        RefreshDisplay();
    }


    // ───────────── Racine n-ième ─────────────

    private void OnRootClicked(object? sender, EventArgs e)
    {
        _engine.SetRootOperator();
        RefreshDisplay();
    }


    // ───────────── Factorielle ─────────────

    private void OnFactorialClicked(object? sender, EventArgs e)
    {
        _engine.Factorial();
        RefreshDisplay();
    }

    // ═════════════════════ Logarithmes et exponentielles ═════════════════════

    // ───────────── Logarithme décimal ─────────────

    private void OnLogClicked(object? sender, EventArgs e)
    {
        _engine.Logarithm();
        RefreshDisplay();
    }


    // ───────────── Logarithme naturel ─────────────

    private void OnNaturalLogClicked(object? sender, EventArgs e)
    {
        _engine.NaturalLogarithm();
        RefreshDisplay();
    }


    // ───────────── Puissance de 10 ─────────────

    private void OnPowerOfTenClicked(object? sender, EventArgs e)
    {
        _engine.PowerOfTen();
        RefreshDisplay();
    }


    // ───────────── Exponentielle naturelle ─────────────

    private void OnExponentialClicked(object? sender, EventArgs e)
    {
        _engine.Exponential();
        RefreshDisplay();
    }
    
    // ═════════════════════ Mode angulaire ═════════════════════

    private void OnAngleModeClicked(object? sender, EventArgs e)
    {
        _engine.ToggleAngleMode();

        AngleModeLabel.Text = _engine.AngleModeText;
        AngleModeButton.Text = _engine.AngleModeText;
    }

    // ═════════════════════ Trigonométrie ═════════════════════

    // ───────────── Sinus ─────────────

    private void OnSineClicked(object? sender, EventArgs e)
    {
        _engine.Sine();
        RefreshDisplay();
    }


    // ───────────── Cosinus ─────────────

    private void OnCosineClicked(object? sender, EventArgs e)
    {
        _engine.Cosine();
        RefreshDisplay();
    }


    // ───────────── Tangente ─────────────

    private void OnTangentClicked(object? sender, EventArgs e)
    {
        _engine.Tangent();
        RefreshDisplay();
    }


    // ───────────── Arc sinus ─────────────

    private void OnArcSineClicked(object? sender, EventArgs e)
    {
        _engine.ArcSine();
        RefreshDisplay();
    }


    // ───────────── Arc cosinus ─────────────

    private void OnArcCosineClicked(object? sender, EventArgs e)
    {
        _engine.ArcCosine();
        RefreshDisplay();
    }


    // ───────────── Arc tangente ─────────────

    private void OnArcTangentClicked(object? sender, EventArgs e)
    {
        _engine.ArcTangent();
        RefreshDisplay();
    }

    // ═════════════════════ Constantes mathématiques ═════════════════════

    // ───────────── Pi ─────────────

    private void OnPiClicked(object? sender, EventArgs e)
    {
        _engine.InputPi();
        RefreshDisplay();
    }


    // ───────────── Constante e ─────────────

    private void OnEClicked(object? sender, EventArgs e)
    {
        _engine.InputE();
        RefreshDisplay();
    }

    // ═════════════════════ Mise à jour de l'affichage ═════════════════════

    private void RefreshDisplay()
    {
        OperationLabel.Text = _engine.ExpressionText;
        ResultLabel.Text = _engine.DisplayText;

        // Un message d'erreur est affiché dans la couleur d'accent principale.
        ResultLabel.TextColor = _engine.HasError
            ? (Color)Resources["AccentPrimary"]
            : (Color)Resources["TextColor"];

        FitResultFont();
    }


    // ───────────── Adaptation de la taille du résultat ─────────────

    private void FitResultFont()
    {
        int length = Math.Max(ResultLabel.Text?.Length ?? 1, 1);

        const double averageCharWidthRatio = 0.62;

        double fitted = _resultMaxWidth / (length * averageCharWidthRatio);

        ResultLabel.FontSize = Math.Clamp(
            fitted,
            22,
            _resultMaxFont);
    }


    // ═════════════════════ Adaptation à l'écran ═════════════════════

    private void AdaptLayout()
    {
        if (Width <= 0 || Height <= 0)
            return;

        bool landscape = Width > Height;

        // Largeur maximale du contenu.
        double contentWidth = Math.Min(Width, MaxContentWidth);

        ContentStack.WidthRequest = contentWidth;

        // Largeur réellement disponible à l'intérieur du contenu.
        double innerWidth = contentWidth - 2 * PagePadding;

        // En paysage, davantage d'espace vertical est disponible
        // pour les touches.
        double reservedHeight = landscape ? 150 : 250;

        double minKey = landscape ? 40 : 52;

        double keyHeight = Math.Clamp(
            (Height - reservedHeight) / 6 - KeySpacing,
            minKey,
            80);

        double keyFont = Math.Clamp(
            keyHeight * 0.38,
            18,
            28);

        // ───────────── Touches du clavier ─────────────

        foreach (Button key in KeypadGrid.Children.OfType<Button>())
        {
            key.HeightRequest = keyHeight;
            key.FontSize = keyFont;
        }

        // ───────────── Touches secondaires ─────────────

        double secondaryWidth =
            (innerWidth - 2 * KeySpacing) / 3;

        foreach (Button key in SecondaryRow.Children.OfType<Button>())
        {
            key.WidthRequest = secondaryWidth;
            key.HeightRequest = keyHeight;
            key.FontSize = keyFont;
        }

        // ───────────── Fonctions scientifiques ─────────────

        double scientificWidth =
            (innerWidth - 4 * KeySpacing) / 5;

        foreach (Button key in ScientificRow.Children.OfType<Button>())
        {
            key.WidthRequest = scientificWidth;
            key.HeightRequest = keyHeight;
            key.FontSize = Math.Clamp(keyHeight * 0.30, 16, 22);
        }

        // ───────────── Deuxième ligne scientifique ─────────────

        double scientificWidth2 =
            (innerWidth - 3 * KeySpacing) / 4;

        foreach (Button key in ScientificRow2.Children.OfType<Button>())
        {
            key.WidthRequest = scientificWidth2;
            key.HeightRequest = keyHeight;
            key.FontSize = Math.Clamp(keyHeight * 0.30, 16, 22);
        }

        // ───────────── Troisième ligne scientifique ─────────────

        double scientificWidth3 =
            (innerWidth - 4 * KeySpacing) / 5;

        foreach (Button key in ScientificRow3.Children.OfType<Button>())
        {
            key.WidthRequest = scientificWidth3;
            key.HeightRequest = keyHeight;
            key.FontSize = Math.Clamp(keyHeight * 0.28, 14, 21);
        }


        // ───────────── Quatrième ligne scientifique ─────────────

        double scientificWidth4 =
            (innerWidth - 2 * KeySpacing) / 3;

        foreach (Button key in ScientificRow4.Children.OfType<Button>())
        {
            key.WidthRequest = scientificWidth4;
            key.HeightRequest = keyHeight;
            key.FontSize = Math.Clamp(keyHeight * 0.30, 16, 22);
        }

        // ───────────── Affichage du résultat ─────────────

        _resultMaxWidth =
            innerWidth - 2 * BorderPadding - 2;

        _resultMaxFont = landscape ? 40 : 56;

        FitResultFont();
    }
}
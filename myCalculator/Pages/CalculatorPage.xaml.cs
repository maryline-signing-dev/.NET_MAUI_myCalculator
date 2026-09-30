using System;

namespace myCalculator.Pages;

public partial class CalculatorPage : ContentPage
{
    public CalculatorPage()
    {
        InitializeComponent();
    }

    // TOUCHES NUMÉRIQUES

    private void OnDigitClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            string digit = button.Text;

            // Test temporaire
            OperationLabel.Text = $"Touche : {digit}";
        }
    }

    // OPÉRATEURS

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            string operatorSymbol = button.Text;

            // Test temporaire
            OperationLabel.Text = $"Opérateur : {operatorSymbol}";
        }
    }

    // NOMBRE DÉCIMAL

    private void OnDecimalClicked(object sender, EventArgs e)
    {
        // Test temporaire
        OperationLabel.Text = "Décimal";
    }

    // CHANGEMENT DE SIGNE

    private void OnSignClicked(object sender, EventArgs e)
    {
        // Test temporaire
        OperationLabel.Text = "Changement de signe";
    }

    // EFFACER LE DERNIER CARACTÈRE

    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        // Test temporaire
        OperationLabel.Text = "Retour arrière";
    }

    // POURCENTAGE

    private void OnPercentClicked(object sender, EventArgs e)
    {
        // Test temporaire
        OperationLabel.Text = "Pourcentage";
    }

    // TOUT EFFACER

    private void OnClearClicked(object sender, EventArgs e)
    {
        // Test temporaire
        OperationLabel.Text = "";
        ResultLabel.Text = "0";
    }

    // ÉGAL

    private void OnEqualsClicked(object sender, EventArgs e)
    {
        // Test temporaire
        OperationLabel.Text = "Calcul";
    }
}


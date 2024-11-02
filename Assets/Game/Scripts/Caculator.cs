using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    public Text displayText; // Assign your display Text or TextMeshPro object here
    private string currentInput = "";
    private float firstNumber = 0;
    private string operation = "";

    public void OnNumberButtonPress(string number)
    {
        currentInput += number;
        UpdateDisplay();
    }

    public void OnOperatorButtonPress(string op)
    {
        firstNumber = float.Parse(currentInput);
        currentInput = "";
        operation = op;
        UpdateDisplay();
    }

    public void OnEqualsButtonPress()
    {
        float secondNumber = float.Parse(currentInput);
        float result = 0;

        switch (operation)
        {
            case "+": result = firstNumber + secondNumber; break;
            case "-": result = firstNumber - secondNumber; break;
            case "*": result = firstNumber * secondNumber; break;
            case "/": result = firstNumber / secondNumber; break;
        }

        currentInput = result.ToString();
        UpdateDisplay();
        currentInput = ""; // Reset for next calculation
    }

    public void OnClearButtonPress()
    {
        currentInput = "";
        firstNumber = 0;
        operation = "";
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayText.text = string.IsNullOrEmpty(currentInput) ? "0" : currentInput;
    }
}
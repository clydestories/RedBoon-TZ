using TMPro;
using UnityEngine;

public class IdBoundCell : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI IdText;

    protected int Id;

    public int CellId
    {
        get { return Id; }
        set 
        {
            Id = value; 
            IdText.text = Id.ToString();
        }
    }
    public TextMeshProUGUI IdTextArea => IdText;
}

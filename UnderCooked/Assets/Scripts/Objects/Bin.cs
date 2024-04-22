using UnityEngine;

public class Bin : MonoBehaviour
{
    GameObject _trash;
    bool _plateRemove = false;


    public PlateReturn PlateReturn;
   

    private void Update()
    {
        if(transform.childCount > 0)
        {
            _trash = transform.GetChild(0).gameObject;
            _trash.GetComponent<Animator>().SetTrigger("binTrigger");
            
            if (_trash.name.Contains("Plate") && !_plateRemove)
            {
                PlateReturn.PlateList.RemoveAt(PlateReturn.PlateList.Count - 1);
                PlateReturn.CurrentPlateNumber--;
                
                _plateRemove = true;
            }
        }

        if (transform.childCount == 0)
            _plateRemove = false;
    }
}

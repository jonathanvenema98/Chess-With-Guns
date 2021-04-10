using System.Collections.Generic;

[System.Serializable]
public class ListData<T>
{
    public List<T> Data { get; set; } 
    
    public ListData() { }

    public ListData(List<T> data)
    {
       Data = data;
    }
}

namespace ParcelDeliveryTrackingAPI.Interfaces
{
    public interface IRepository<T>
    {
        //Task CreateNewRowAsync(T entity);    //  method for creating a new row
        List<T> GetAllRows();                  //  method for getting all rows
        T GetRowById(int id);                  //  method for getting a row by ID
        (T updatedItem, string message) UpdateRow(T entity);         //  method for updating a row
        bool DeleteRow(int id);                //  method for deleting a row
    }
}

namespace TestTaskJuniorNETDeveloper.Interfaces
{
    public interface IRandomDataGeneratorcs
    {
        string RandomChars(string chars, int length);
        DateOnly RandomDay();
        double RandomDouble(double min, double max, int decimalPlaces);
        int RandomNumber(int min, int max);
    }
}
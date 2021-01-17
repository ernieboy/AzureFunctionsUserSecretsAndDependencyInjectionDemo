namespace FunctionApp1.Model
{
    public class Customer
    {

        public Customer()
        {

        }

        public Customer(int age, string gender)
        {
            Age = age;
            Gender = gender;
        }

        public int Age { get; private set; }

        public string Gender { get; private set; }  
    }
}

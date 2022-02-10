using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AS_Assignment.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Email Address is required!"), DataType(DataType.EmailAddress), EmailAddress]
        public String email { get; set; }
        [Required(ErrorMessage = "Password is required!"), RegularExpression("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{12,}$", ErrorMessage = "Password is not strong enough! Password requires 2 Capital letters, 3 small letters, 2 digits, a special character, and has to be at least 12 characters long!"), DataType(DataType.Password), MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        public String password { get; set; }
        [Required, RegularExpression("^[STFG]\\d{7}[A-Z]$", ErrorMessage = "NRIC is not valid.")]
        public String NRIC { get; set; }
        [Required(ErrorMessage = "First Name is required!")]
        public String firstName { get; set; }
        [Required(ErrorMessage = "Last Name is required!")]
        public String lastName { get; set; }
        [Required, RegularExpression("\\b(?:\\d[-]*?){13,16}\\b", ErrorMessage = "Credit Card Number is not valid.")]
        public String creditCard { get; set; }
        [Required, RegularExpression("^[0-9]{3,4}$", ErrorMessage = "CVC Number is not valid.")]
        public String cvc { get; set; }
        [Required(ErrorMessage = "Date of Birth is required!"), DataType(DataType.Date)]
        public DateTime birthDate { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime dateTimeRegistered { get; set; }
        public String salt { get; set; }
        public int lockedOut { get; set; }
        public Nullable<DateTime> lockEnd { get; set; }
        public int tries { get; set; }
        public byte[] avatar { get; set; }
        public int isAdmin { get; set; }
        private String roles { get; set; }
        private String pfp { get; set; }
        private int deleted { get; set; }

        public bool isUserAdmin()
        {
            return (isAdmin == 1);
        }

        public String getFullName()
        {
            return firstName + " " + lastName;
        }

        public bool setUserAdmin(Object kys)
        {
            if (kys.GetType() == typeof(int))
            {
                int fuck = int.Parse(kys.ToString());

                isAdmin = (fuck > 1) ? 1 : 0;
                return true;
            }
            else if (kys.GetType() == typeof(bool))
            {
                bool a = bool.Parse(kys.ToString());

                isAdmin = a ? 1 : 0;

                return true;
            }
            else if (kys.GetType() == typeof(string))
            {
                string fuckthisman = kys.ToString().ToLower();

                if (fuckthisman == "0" || fuckthisman == "1")
                    isAdmin = int.Parse(fuckthisman);
                else
                    isAdmin = (String.IsNullOrEmpty(fuckthisman)) ? 0 : 1;
                return true;
            }

            return false;
        }

        public List<String> getRoles()
        {
            if (String.IsNullOrEmpty(roles))
                return null;
            else
                return roles.Split(',').ToList();
        }

        public void addRole(Object roleID)
        {
            if (String.IsNullOrEmpty(roles))
                roles = roleID.ToString();
            else
                roles += "," + roleID.ToString();
        }

        public bool removeRole(Object roleID)
        {
            List<String> input = getRoles();
            List<String> output = new List<String>();

            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].ToLower() != roleID.ToString().ToLower())
                {
                    output.Add(input[i]);
                }
            }

            bool b = input.SequenceEqual(output);

            if (!b)
                roles = String.Join(",", output);

            return b;
        }

        public override String ToString()
        {
            return firstName + ":" + lastName + ":" + email + ":" + birthDate.ToString();
        }
    }
}

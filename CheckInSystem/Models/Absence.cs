using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem.Models
{
    public class Absence
    {
        public int _id { get; private set; }
        public int _employeeId { get; private set; }
        public DateTime _fromDate { get; private set; }
        public DateTime _toDate { get; private set; }
        public string _note { get; private set; }

        public Absence() 
        { }

        public void InsertAbsence(DateTime _fromDate,DateTime _toDate, string _note)
        {

        }
        public void EditAbsence(DateTime _fromDate, DateTime _toDate, string _note)
        {

        }
        public void DeleteAbsence(int _id) 
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GEM.Utilities
{
    public class HelperEnum
    {
        public enum Journey_Information
        {
            Delivery_Team = 1,
            Product_Team = 2,
            Strategy_Team = 3,
            Enterprise_Team = 4
        }

        public static List<string> JourneyInformation(bool fullname = false)
        {
            var journey = new List<string>();
            if (fullname)
            {
                journey.Add("Delivery Team - # Deliver Value to Customer");
                journey.Add("Product Team - # Vision to Challange the Norm");
                journey.Add("Enterprise Team - # Protectors and Gaurdians of Value");
                journey.Add("Strategy Team - # Guides Strategy and Value");
            }
            else
            {
                journey.Add("Delivery Team");
                journey.Add("Product Team");
                journey.Add("Enterprise Team");
                journey.Add("Strategy Team");
            }

            return journey;
        }

    }
}
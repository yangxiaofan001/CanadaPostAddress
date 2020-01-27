using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressLookup
{
    class Program
    {
        static string apikey = "DJ78-YZ96-EJ47-RK77";
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Enter an address, enter to exit:");
                string searchAddress = Console.ReadLine();
                if (string.IsNullOrEmpty(searchAddress))
                    break;

                System.Data.DataSet dsV1Find = AddressCompleteInteractiveAutoCompletev100(
                    apikey,
                    searchAddress,
                    "",
                    100,
                    "CA",
                    "en-ca"
                );

                Console.WriteLine("V1 returned " + dsV1Find.Tables[0].Rows.Count + " addresses");
                foreach (System.Data.DataRow dr in dsV1Find.Tables[0].Rows)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(dr["Text"].ToString() + dr["Description"].ToString() + "; retrieveable: " + dr["IsRetrievable"].ToString());
                    Console.WriteLine("Fields:");

                    try
                    {
                        System.Data.DataSet dsV1Retrieve = AddressCompleteInteractiveRetrieveByIdv100("DJ78-YZ96-EJ47-RK77", dr["Id"].ToString(), "");
                        foreach (System.Data.DataRow dr1 in dsV1Retrieve.Tables[0].Rows)
                        {
                            Console.WriteLine("\t" + dr1["FieldGroup"].ToString() + "." + dr1["FieldName"].ToString() + " = " + dr1["FormattedValue"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Unable to retrieve");
                    }
                }

                Console.WriteLine();
                

                System.Data.DataSet dsV2Find = AddressCompleteInteractiveFindv210(apikey, searchAddress, "", "Everything", "CAN", "en", 7, 7);
                Console.WriteLine("V2 returned " + dsV2Find.Tables[0].Rows.Count + " addresses");

                Console.WriteLine();
                foreach (System.Data.DataRow dr in dsV2Find.Tables[0].Rows)
                {
                    Console.WriteLine();
                    Console.WriteLine("Row:");
                    foreach (System.Data.DataColumn dc in dsV2Find.Tables[0].Columns)
                    {
                        Console.WriteLine(dc.ColumnName + " = " + dr[dc].ToString());
                    }

                    if (dr["Next"].ToString() == "Retrieve")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Retrieving address");
                        System.Data.DataSet dsV2Retrieve = AddressCompleteInteractiveRetrievev211(apikey, dr["Id"].ToString());
                        Console.WriteLine("Retrieved address, table " + dsV2Retrieve.Tables[0].TableName);
                        if (dsV2Find.Tables[0].Rows.Count > 0)
                        {
                            foreach (System.Data.DataColumn dc1 in dsV2Retrieve.Tables[0].Columns)
                            {
                                Console.WriteLine("\t" + dc1.ColumnName + " = " + dsV2Retrieve.Tables[0].Rows[0][dc1].ToString());
                            }
                        }
                    }
                }
            }
        }

        static System.Data.DataSet AddressCompleteInteractiveFindv210(string key, string searchterm, string lastid, string searchfor, string country, string languagepreference, int maxsuggestions, int maxresults)
        {
            //Build the url
            var url = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/Find/v2.10/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&SearchTerm=" + System.Web.HttpUtility.UrlEncode(searchterm);
            url += "&LastId=" + System.Web.HttpUtility.UrlEncode(lastid);
            url += "&SearchFor=" + System.Web.HttpUtility.UrlEncode(searchfor);
            url += "&Country=" + System.Web.HttpUtility.UrlEncode(country);
            url += "&LanguagePreference=" + System.Web.HttpUtility.UrlEncode(languagepreference);
            url += "&MaxSuggestions=" + System.Web.HttpUtility.UrlEncode(maxsuggestions.ToString(CultureInfo.InvariantCulture));
            url += "&MaxResults=" + System.Web.HttpUtility.UrlEncode(maxresults.ToString(CultureInfo.InvariantCulture));

            //Create the dataset
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //Id
            //Text
            //Highlight
            //Cursor
            //Description
            //Next
        }

        static System.Data.DataSet AddressCompleteInteractiveRetrievev211(string key, string id)
        {
            //Build the url
            var url = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/Retrieve/v2.11/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&Id=" + System.Web.HttpUtility.UrlEncode(id);

            //Create the dataset
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);

            //Check for an error
            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 && dataSet.Tables[0].Columns[0].ColumnName == "Error")
                throw new Exception(dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //Id
            //DomesticId
            //Language
            //LanguageAlternatives
            //Department
            //Company
            //SubBuilding
            //BuildingNumber
            //BuildingName
            //SecondaryStreet
            //Street
            //Block
            //Neighbourhood
            //District
            //City
            //Line1
            //Line2
            //Line3
            //Line4
            //Line5
            //AdminAreaName
            //AdminAreaCode
            //Province
            //ProvinceName
            //ProvinceCode
            //PostalCode
            //CountryName
            //CountryIso2
            //CountryIso3
            //CountryIsoNumber
            //SortingNumber1
            //SortingNumber2
            //Barcode
            //POBoxNumber
            //Label
            //Type
            //DataLevel
        }

        static System.Data.DataSet AddressCompleteInteractiveAutoCompletev100(string key, string searchterm, string location, int locationaccuracy, string country, string languagepreference)
        {
            //Build the url
            var url = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/AutoComplete/v1.00/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&SearchTerm=" + System.Web.HttpUtility.UrlEncode(searchterm);
            url += "&Location=" + System.Web.HttpUtility.UrlEncode(location);
            url += "&LocationAccuracy=" + System.Web.HttpUtility.UrlEncode(locationaccuracy.ToString(CultureInfo.InvariantCulture));
            url += "&Country=" + System.Web.HttpUtility.UrlEncode(country);
            url += "&LanguagePreference=" + System.Web.HttpUtility.UrlEncode(languagepreference);

            //Create the dataset
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);

            //Check for an error
            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 && dataSet.Tables[0].Columns[0].ColumnName == "Error")
                throw new Exception(dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //Id
            //Text
            //Highlight
            //Description
            //IsRetrievable
        }

        static System.Data.DataSet AddressCompleteInteractiveRetrieveByIdv100(string key, string id, string application)
        {
            //Build the url
            var url = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/RetrieveById/v1.00/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&Id=" + System.Web.HttpUtility.UrlEncode(id);
            url += "&Application=" + System.Web.HttpUtility.UrlEncode(application);

            //Create the dataset
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);

            //Check for an error
            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 && dataSet.Tables[0].Columns[0].ColumnName == "Error")
                throw new Exception(dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //FieldGroup
            //FieldName
            //FormattedValue
            //FieldType
            //FieldSequence
            //Language
        }

    }
}

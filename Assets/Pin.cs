//using System;


//[Serializable]
//public class Pin : ConnectibleBase
//{

//    public string parentConnectorId;

//    [NonSerialized]
//    public ConnectorData parentConnector;

//    public string name;

//    public new string Id
//    {
//        get
//        {
//            if (parentConnector == null)
//                return "MISSING";

//            _id = $"{parentConnector.id}.{Id}";
//            return _id;
//        }
//        set
//        {
//            _id = value;
//        }
//        // WARNING: Choose if we manage wether to consider this field to be editable from the config file or not.
//        //set
//        //{
//        //    // TODO: Here we should find the connector, not just assigning it, to be precise
//        //    parentConnector.id = value.Split('.')[0];

//        //    id = value.Split('.')[1];
//        //}
//    }

//    public void SetConnector()
//    {
//        //parentConnector = 
//    }
//}


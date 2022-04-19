using System;
using System.Collections.Generic;

[Serializable]
public class Connection
{
    public string connectionId;
    public string ConnectionFromId;
    public List<string> ConnectionToId;
}

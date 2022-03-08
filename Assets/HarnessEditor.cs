using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HarnessEditor : MonoBehaviour
{
    public HarnessDictSO HarnessDictSO;
    public HarnessSO HarnessSO;

    public GameObject ConnectorPrefab;
    public Transform HarnessParent;

    // Start is called before the first frame update
    void Start()
    {
        HarnessParent ??= transform;

        HarnessDictSO.harnessIdDictEntryDict = HarnessSO.connectors.ToDictionary(c => c.id, c => new HarnessDictEntry() { connectorData = c });
        GenerateConnectors();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddConnector(Vector3 position, ConnectorSO connectorSO)
    {
        ConnectorData connector = new ConnectorData()
        {
            connector = connectorSO,
            id = Guid.NewGuid().ToString("N"),
            Pins = new List<Pin>(connectorSO.PinConfig.PinCount),
            name = connectorSO.name,
            position = position
        };

        HarnessSO.connectors.Add(connector);
        GenerateConnector(HarnessParent, connector);
    }

    void GenerateConnectors()
    {
        var parent = HarnessParent;

        foreach (var connector in HarnessSO.connectors)
        {
            GenerateConnector(parent, connector);
        }
    }

    private void GenerateConnector(Transform parent, ConnectorData connector)
    {

        ConnectorBuilder builder;
        GameObject connectorGO;

        connectorGO = Instantiate(ConnectorPrefab, connector.position, Quaternion.identity, parent);
        builder = connectorGO.GetComponent<ConnectorBuilder>();
        builder.SetData(connector);
    }

    void SaveChanges()
    {
        Debug.Log("Saving Harness" + HarnessSO.name);
    }

    private void OnDrawGizmos()
    {
        foreach (var connection in HarnessSO.connections)
        {
            var fromConnectible = HarnessSO.connectors.SelectMany(conn => conn.Pins).SingleOrDefault(pin => pin.Id == connection.ConnectionFromId);
            var toConnectibles = HarnessSO.connectors.SelectMany(conn => conn.Pins).Where(pin => connection.ConnectionToId.Contains(pin.Id));
            if (fromConnectible == null)
                return;
            foreach (var toConn in toConnectibles)
            {
                Gizmos.DrawLine(fromConnectible.position, toConn.position);
            }
        }
    }
}

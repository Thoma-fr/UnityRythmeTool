using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioVisulazerSPawner : MonoBehaviour
{
    [SerializeField] private bool _isMirroring;
    [SerializeField] private bool _isReversed;
    [SerializeField] private float _maxBias;
    [SerializeField] private bool _isCircle;
    public GameObject Visualiazer;
    [SerializeField]private int numberToDisplay;

    private List<GameObject> spawned = new List<GameObject>();
    private List<Vector3> pos = new List<Vector3>();
    [SerializeField] private Vector3 offset;
    [SerializeField] float _radius;
    private Vector3 currentOfset;
    [SerializeField]private Vector3 _scale=Vector3.one;
    [SerializeField] private Vector3 _Beatscale = new Vector3(1,5,5);
    [Button]
    public void SpawnVisualizer()
    {
        foreach (var item in spawned)
            {
                DestroyImmediate(item.gameObject);
            }
        spawned.Clear();


        if (!_isCircle)
            SpawnInLine();
        else
            SpawnIncircle();

    }
    private void SpawnInLine()
    {
        float biasToAttribute;
        float curBias;
        float biasUnit = _maxBias / numberToDisplay;
        if (_isReversed)
            biasToAttribute = _maxBias;
        else 
            biasToAttribute = biasUnit;


        curBias = biasToAttribute;
        for (int i = 0; i < numberToDisplay; i++)
        {
            GameObject go = Instantiate(Visualiazer, transform.position + currentOfset, transform.rotation,transform);

            
            if (go.TryGetComponent(out AudioSync sync))
            {
                sync.bias = curBias;
                go.transform.localScale = _scale;
                go.GetComponent<ScaleOnSpectrum>().restScale = _scale;
                go.GetComponent<ScaleOnSpectrum>().beatScale = _Beatscale;
                spawned.Add(go);
            }
            if (_isMirroring)
            {
                GameObject go2 = Instantiate(Visualiazer, transform.position - currentOfset, transform.rotation, transform);
                if (go2.TryGetComponent(out AudioSync sync2))
                {
                    sync2.bias = curBias;
                    go2.transform.localScale = _scale;
                    go2.GetComponent<ScaleOnSpectrum>().restScale = _scale;
                    go2.GetComponent<ScaleOnSpectrum>().beatScale = _Beatscale;
                    spawned.Add(go2);
                }
            }
            currentOfset = currentOfset + offset;

            if(_isReversed)
                curBias -= biasUnit;
            else
                curBias += biasUnit;
        }
        currentOfset = Vector3.zero;
    }
    private void OnDrawGizmosSelected()
    {
        if (!_isCircle)
            DrawGizmoLineSquare();
        else
            DrawGizmoCircle();
    }
    private void DrawGizmoLineSquare()
    {
        for (int i = 0; i < numberToDisplay; i++)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawMesh(Visualiazer.GetComponentInChildren<MeshFilter>().sharedMesh, 0, transform.position + currentOfset, transform.rotation, _scale);
            //Gizmos.DrawMesh(transform.position+ currentOfset, Visualiazer.transform.localScale);
            if (_isMirroring)
                Gizmos.DrawMesh(Visualiazer.GetComponentInChildren<MeshFilter>().sharedMesh, 0, transform.position - currentOfset, transform.rotation, _scale);
            currentOfset = currentOfset + offset;
        }
        currentOfset = Vector3.zero;
    }
    private void DrawGizmoCircle()
    {
        float angleIncrement = 360f / numberToDisplay;

        Vector3 startPosition = transform.up * _radius;

        for (int i = 0; i < numberToDisplay; i++) 
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angleIncrement, transform.forward);
            Vector3 pointPosition = transform.position + rotation * startPosition;
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawMesh(Visualiazer.GetComponentInChildren<MeshFilter>().sharedMesh, 0, pointPosition, rotation, _scale);

        }

    }
    private void SpawnIncircle()
    {
        float angleIncrement = 180f / (numberToDisplay / 2); // 180� pour un c�t�, divis� par le nombre de points d'un c�t�.
        Vector3 startPosition = transform.up * _radius;

        float biasToAttribute = _maxBias / numberToDisplay; // Bias divis� par le nombre total d'�l�ments.
        float curBias = biasToAttribute;

        for (int i = 0; i < numberToDisplay / 2; i++)
        {
            // Calcul du point � gauche
            Quaternion rotation = Quaternion.AngleAxis(i * angleIncrement, transform.forward);
            Vector3 pointPositionLeft = transform.position + rotation * startPosition;
            GameObject goLeft = Instantiate(Visualiazer, pointPositionLeft, rotation, transform);

            if (goLeft.TryGetComponent(out AudioSync syncLeft))
            {
                syncLeft.bias = curBias;
                goLeft.transform.localScale = -_scale; // �chelle normale pour le c�t� gauche
                
                goLeft.GetComponent<ScaleOnSpectrum>().restScale = -_scale;
                goLeft.GetComponent<ScaleOnSpectrum>().beatScale =- _Beatscale;
                spawned.Add(goLeft);
            }

            // Calcul du point en miroir (� droite)
            Quaternion mirrorRotation = Quaternion.AngleAxis(-i * angleIncrement, transform.forward); // Rotation oppos�e
            Vector3 pointPositionRight = transform.position + mirrorRotation * startPosition;
            GameObject goRight = Instantiate(Visualiazer, pointPositionRight, mirrorRotation, transform);

            if (goRight.TryGetComponent(out AudioSync syncRight))
            {
                syncRight.bias = curBias;
                goRight.transform.localScale = -_scale; // �chelle normale pour le c�t� miroir
                goRight.GetComponent<ScaleOnSpectrum>().restScale = -_scale;
                goRight.GetComponent<ScaleOnSpectrum>().beatScale = _Beatscale;
                spawned.Add(goRight);
            }

            // Incr�mentation du bias pour les deux c�t�s
            curBias += biasToAttribute;
        }
    }
}

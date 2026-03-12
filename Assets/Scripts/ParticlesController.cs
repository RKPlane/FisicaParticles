using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticlesController : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject goParticulaPrefab;

    [Header("Config")]
    public int iNumeroParticulas = 20;
    public float fVelocidadInicial = 5f;
    public float fAnguloInicial = 45f;
    public float fTiempoVida = 3f;
    public float fGravedad = 9.8f;

    [Header("Random")]
    public float fVariacionVelocidad = 2f;
    public float fVariacionAngulo = 30f;
    public float fVariacionVida = 1f;

    private List<GameObject> lGoParticulas = new List<GameObject>();

    [Header("Input Settings")]
    public InputActionAsset inputActions;
    private InputAction g_GenerateAction;

    private void Awake()
    {
        g_GenerateAction = InputSystem.actions.FindAction("Generate");
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        if (g_GenerateAction.WasPressedThisFrame())
        {
            EliminarTodasLasParticulas();
            CreateParticleExplotion();
        }

        ActualizarYLimpiarParticulas();
    }

    void CreateParticleExplotion()
    {
        for (int iIndice = 0; iIndice < iNumeroParticulas; iIndice++)
        {
            GameObject goParticula = Instantiate(goParticulaPrefab, transform.position, Quaternion.identity);
            Particle pParticula = goParticula.GetComponent<Particle>();

            pParticula.fVelocidadInicial = fVelocidadInicial + Random.Range(-fVariacionVelocidad, fVariacionVelocidad);
            pParticula.fAnguloInicial = fAnguloInicial + Random.Range(-fVariacionAngulo, fVariacionAngulo);
            pParticula.fTiempoVidaMaximo = fTiempoVida + Random.Range(-fVariacionVida, fVariacionVida);
            pParticula.fGravedad = fGravedad;

            pParticula.Inicializar(transform.position);
            lGoParticulas.Add(goParticula);
        }
    }

    void UpdateParticlePosition(Particle pParticula, float fTiempo)
    {
        pParticula.fTiempoActivo += fTiempo;
        pParticula.transform.position = pParticula.ObtenerPosicionEnTiempo(pParticula.fTiempoActivo);
    }

    void ActualizarYLimpiarParticulas()
    {
        List<GameObject> lGoEliminar = new List<GameObject>();

        foreach (GameObject goParticula in lGoParticulas)
        {
            if (goParticula == null) continue;

            Particle pParticula = goParticula.GetComponent<Particle>();
            UpdateParticlePosition(pParticula, Time.deltaTime);

            if (pParticula.fTiempoActivo >= pParticula.fTiempoVidaMaximo)
                lGoEliminar.Add(goParticula);
        }

        foreach (GameObject goEliminar in lGoEliminar)
        {
            lGoParticulas.Remove(goEliminar);
            Destroy(goEliminar);
        }
    }

    void EliminarTodasLasParticulas()
    {
        foreach (GameObject goParticula in lGoParticulas)
            if (goParticula != null) Destroy(goParticula);
        lGoParticulas.Clear();
    }
}
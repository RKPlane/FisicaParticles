using UnityEngine;

public class Particle : MonoBehaviour
{
    public float fVelocidadInicial;
    public float fAnguloInicial;
    public float fTiempoVidaMaximo;
    public float fTiempoActivo;
    public float fGravedad;

    private Vector2 v2VelocidadInicial;
    private Vector3 v3PosicionOrigen;

    public void Inicializar(Vector3 v3PosicionSpawn)
    {
        v3PosicionOrigen = v3PosicionSpawn;
        transform.position = v3PosicionSpawn;
        fTiempoActivo = 0f;

        float fAnguloRad = fAnguloInicial * Mathf.Deg2Rad;
        v2VelocidadInicial = new Vector2(
            fVelocidadInicial * Mathf.Cos(fAnguloRad),
            fVelocidadInicial * Mathf.Sin(fAnguloRad)
        );
    }

    public Vector3 ObtenerPosicionEnTiempo(float fTiempo)
    {
        float fX = v3PosicionOrigen.x + v2VelocidadInicial.x * fTiempo;
        float fY = v3PosicionOrigen.y + v2VelocidadInicial.y * fTiempo - 0.5f * fGravedad * fTiempo * fTiempo;
        return new Vector3(fX, fY, v3PosicionOrigen.z);
    }
}

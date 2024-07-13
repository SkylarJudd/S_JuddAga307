using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using System;


public class FireLazer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] ThirdPersonMovementScript thirdPersonMovementScript;
    
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private CinemachineCamera defultVirtualCamera;
    [SerializeField] float defaultSensitivity = 1f;
    [SerializeField] float aimSensitivity = 0.1f;
    [SerializeField] float lerpToZoomStopSensitivity = 1f;
    [SerializeField] float lerpToZoomRestartSensitivity = 180f;
    [SerializeField] float rotateCamToPlayerWhenZoomedSpeed = 5f;
    [SerializeField] float rotatePlayerToCamWhenZoomedSpeed = 20f;

    float camX;
    float camY;
    float camOverShoot;
    float sensitivity;

    Vector3 mouseWorldPosition = Vector3.zero;
    Vector3 aimDirection;

    bool lerpCam;
    bool lerp = true;
    Coroutine lerpCamCoroutine;
    CinemachineOrbitalFollow aimVirtualCameraOrbital;
    CinemachineOrbitalFollow defultVirtualCameraOrbital;

    [Header("Shoot")]
    [SerializeField] GunData[] Guns;
    [SerializeField] LayerMask ShootLayerMask = new LayerMask();
    [SerializeField] Transform debugTransform;

    private float currentShootCoolDown;
    private int ActiveGun;


    [Serializable]
   private class GunData
    {
        public GameObject projectilePrefab;
        public Transform[] shootTranforms;
        public float shootCoolDown = 0.5f;
        public float projectileSpeed = 1000f;

    }



    private void Awake()
    {

    }

    private void Start()
    {

        defultVirtualCameraOrbital = defultVirtualCamera.GetComponent<CinemachineOrbitalFollow>();
        aimVirtualCameraOrbital = aimVirtualCamera.GetComponent<CinemachineOrbitalFollow>();

        camX = defultVirtualCameraOrbital.HorizontalAxis.Value;
        camY = defultVirtualCameraOrbital.VerticalAxis.Value;
    }

    private void LateUpdate()
    {

    }

    public void shoot(InputAction.CallbackContext _context)
    {
        if (_context.ReadValue<float>() == 1 && currentShootCoolDown <= 0)
        {
            Transform spawnBulletPos = Guns[ActiveGun].shootTranforms[UnityEngine.Random.Range(0, Guns[ActiveGun].shootTranforms.Length)];
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPos.position).normalized;

            Instantiate(Guns[ActiveGun].projectilePrefab, spawnBulletPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
            currentShootCoolDown = Guns[ActiveGun].shootCoolDown;
        }
    }

    public void ChangeWepons(InputAction.CallbackContext _context)
    {
        print("MouseScroll" + _context.ReadValue<float>());
        if (_context.ReadValue<float>() < 0)
        {
            ActiveGun++;
            if (ActiveGun >= Guns.Length)
            {
                ActiveGun = 0;
            }
        }
        else if (_context.ReadValue<float>() > 0)
        {
            ActiveGun--;
            if (ActiveGun < 0)
            {
                ActiveGun = Guns.Length - 1;
            }
        }
    }

    public void setWeponOne(InputAction.CallbackContext _context)
    {
        if (_context.ReadValue<float>() == 1)
        {
            ActiveGun = 0;
        }
    }
    public void setWeponTwo(InputAction.CallbackContext _context)
    {
        if (_context.ReadValue<float>() == 1)
        {
            ActiveGun = 1;
        }
    }
    public void setWeponThree(InputAction.CallbackContext _context)
    {
        if (_context.ReadValue<float>() == 1)
        {
            ActiveGun = 2;
        }
    }

    public void PlayerLook(InputAction.CallbackContext _context)
    {
        Vector2 value = _context.ReadValue<Vector2>();

        camX += value.x * sensitivity * 0.1f;
        camY += value.y * sensitivity * 0.1f;

        mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, ShootLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if(lerpCam)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = player.transform.position.y;
            Vector3 aimDirection = (worldAimTarget - player.transform.position).normalized;

            player.transform.forward = Vector3.Lerp(player.transform.forward, aimDirection, Time.deltaTime * rotatePlayerToCamWhenZoomedSpeed);
        }

        camY = Mathf.Clamp(camY, -45, 10);
        if (camX < -180)
        {
            camOverShoot = -180 - camX;
            camX = 180 + camOverShoot;
        }
        else if (camX > 180)
        {
            camOverShoot = camX - 180;
            camX = -180 + camOverShoot;
        }

        defultVirtualCameraOrbital.HorizontalAxis.Value = camX;
        defultVirtualCameraOrbital.VerticalAxis.Value = -camY;
    }

    public void AimZoom(InputAction.CallbackContext _context)
    {
        if (_context.ReadValue<float>() == 1)
        {
            aimVirtualCameraOrbital.HorizontalAxis.Value = defultVirtualCameraOrbital.HorizontalAxis.Value;
            sensitivity = aimSensitivity;
            aimVirtualCamera.Priority = 2;
            lerpCam = true;
            lerp = true;
            thirdPersonMovementScript.SetRotationOnMove(false);



            //StartLerpCamera();
        }
        else if (_context.ReadValue<float>() == 0)
        {
            defultVirtualCameraOrbital.HorizontalAxis.Value = aimVirtualCameraOrbital.HorizontalAxis.Value;
            sensitivity = defaultSensitivity;
            aimVirtualCamera.Priority = 0;
            lerpCam = false;
            thirdPersonMovementScript.SetRotationOnMove(true);
            //StopLerpCamera();
        }
    }

    public void StartLerpCamera()
    {
        if (lerpCamCoroutine != null)
        {
            StopCoroutine(lerpCamCoroutine);
        }
        lerpCam = true;
        lerpCamCoroutine = StartCoroutine(LerpPlayerView());
    }

    public void StopLerpCamera()
    {
        if (lerpCamCoroutine != null)
        {
            StopCoroutine(lerpCamCoroutine);
            lerpCamCoroutine = null;
        }
        lerpCam = false;
    }
    private IEnumerator LerpPlayerView()
    {
        while (lerpCam == true)
        {
            float targetRotation = NormalizeAngle(player.transform.eulerAngles.y);
            camX = NormalizeAngle(camX);

            print("Target Rotation: " + targetRotation);
            print("Current camX (Normalized): " + camX);

            camX = Mathf.Lerp(camX, targetRotation, rotateCamToPlayerWhenZoomedSpeed * Time.deltaTime);

            print("Lerped camX: " + camX);

            

            // If statement that happens when camX reaches the target rotation
            if (Mathf.Abs(camX - targetRotation) < lerpToZoomStopSensitivity)
            {
                lerp = false;
            }

            // If statement that happens when camX moves too far away from the target value
            if (Mathf.Abs(camX - targetRotation) > lerpToZoomRestartSensitivity)
            {
                lerp = true;
            }

            if (lerp)
            {
                aimVirtualCameraOrbital.HorizontalAxis.Value = camX;
            }
            
            yield return new WaitForEndOfFrame();
        }

    }


    private float NormalizeAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }
        while (angle < -180f)
        {
            angle += 360f;
        }
        return angle;
    }


    private void Update()
    {
        if (currentShootCoolDown > 0)
        {
            currentShootCoolDown -= Time.deltaTime;
        }
        else
        {
            currentShootCoolDown = 0;
        }
    }
}

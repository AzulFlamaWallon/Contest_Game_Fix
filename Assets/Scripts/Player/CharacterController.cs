﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Network.Data;
using Greyzone.GUI;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>버튼 트리거 이벤트(버튼명, 누름/뗌 여부)</summary>
public class Event_Button_Triggered : UnityEvent<string, bool> { }
/// <summary>툴 변경 이벤트(변경한 툴 인덱스)</summary>
public class Event_Tool_Changed : UnityEvent<int> { }
/// <summary>피해 이벤트</summary>
public class Event_Damaged : UnityEvent<int> { }
/// <summary>스턴 이벤트</summary>
public class Event_Stunned : UnityEvent<int> { }
/// <summary>스킬 이벤트</summary>
public class Event_RoleSkill_Toggle : UnityEvent<bool> { }

/// <summary>
/// 200913 주현킴
/// 캐릭터 컨트롤러 베이스 클래스
/// </summary>
public class CharacterController : MonoBehaviour
{
    // 입출력
    protected Manager_Input InputManager;   // 입력 관리자
    private User_Input      m_Output;       // 서버로부터 받은 입력값

    [Header("네트워크/프로필")]
    public  User_Profile                m_MyProfile;   // 캐릭터 프로필
    private UnityAction<User_Profile[]> m_PlayerInputEvts; // 입력 수신 이벤트
    private UnityAction<Session_RoundData, User_Profile[]> m_PlayerUpdatePosEvts; // 위치 갱신 이벤트
    private UnityAction<UInt16, UInt16> m_PlayerDamageEvts; // 피해 이벤트
    private UnityAction<UInt16, UInt16> m_PlayerStunEvts; // 스턴 이벤트
    

    // 이벤트
    public Event_Button_Triggered e_Triggered = new Event_Button_Triggered();
    public Event_Tool_Changed     e_ToolChanged = new Event_Tool_Changed();
    public Event_Damaged          e_Damaged = new Event_Damaged();
    public Event_Stunned          e_Stunned = new Event_Stunned();
    public Event_RoleSkill_Toggle e_RoleSkill_Toggle = new Event_RoleSkill_Toggle();
    

    // 프로퍼티
    public User_Profile ClientProfile
    {
        get => Manager_Ingame.Instance.m_Client_Profile;
        protected set => Manager_Ingame.Instance.m_Client_Profile = value;
    }

    [Header("캐릭터 스탯")]
    public int   battery = 10000;
    public float moveSpeed;
    public bool  m_Is_Stunned = false;

    [Header("캐릭터 물리메테리얼")]
    public PhysicMaterial noFriction;     //프릭션 X
    public PhysicMaterial fullFriction; // 프릭션 O

    [Header("캐릭터 무기")]
    public Transform m_ToolAxis;
    public Tool[]    m_Tools = new Tool[4]; // 툴 리스트

    [Header("캐릭터 좌표")]
    public Transform m_CameraAxis;
    public Vector3   m_Before_Position;
    public const float ASCENDING_LIMIT = 0.6f;

    [Header("아이템 루팅")]
    [CustomRange(0, 20.0f)]
    public float acquireDist = 10.0f;
    public float itemSearchDuration = 1.0f;
    public LayerMask ItemLayer;
    private Vector3[] hitPos;
    private Vector3[] dir;

    private bool IsHit = false;

    public PlayerState playerState;

    Renderer[] m_Renderers;

    IEnumerator Start()
    {
        // 인풋 매니저 싱글톤 대기
        while (Manager_Input.Instance == null)
            yield return new WaitForEndOfFrame();
        InputManager = Manager_Input.Instance;

        // 입력 이벤트 등록
        RegisterEvents();
        
        // 아이템 체크 로직
        //StartCoroutine(Update_FieldOnItem());

        // 프로필에 해당하는 툴 등록
        RegisterTools();
    }

    void RegisterEvents()
    {
        m_PlayerInputEvts     = new UnityAction<User_Profile[]>(When_Player_Input);
        m_PlayerUpdatePosEvts = new UnityAction<Session_RoundData, User_Profile[]>(When_Player_UpdatePosition);
        m_PlayerDamageEvts    = new UnityAction<ushort, ushort>(Damage);
        m_PlayerStunEvts      = new UnityAction<ushort, ushort>(Stun);       

        if (Manager_Network.Instance == null)
        {
            // 네트워크 매니저 없음 -> 로컬 디버그 모드
            Manager_Ingame.Instance.e_FakeInput.AddListener(m_PlayerInputEvts);
        }
        else
        {
            // 네트워크 매니저 있음 -> 서버 거쳐서 인풋 받기
            Manager_Network.Instance.e_PlayerInput.AddListener(m_PlayerInputEvts);
            Manager_Network.Instance.e_HeartBeat.AddListener(m_PlayerUpdatePosEvts);
            Manager_Network.Instance.e_PlayerHit.AddListener(m_PlayerDamageEvts);
            Manager_Network.Instance.e_PlayerStun.AddListener(m_PlayerStunEvts);
        }  
        
        
    }

    void RegisterTools()
    {
        m_Tools[0] = MakeTool(m_MyProfile.Tool_1);
        m_Tools[1] = MakeTool(m_MyProfile.Tool_2);
        m_Tools[2] = MakeTool(m_MyProfile.Tool_3);
        m_Tools[3] = MakeTool(m_MyProfile.Tool_4);
        ChangeTool(2);
        ChangeTool(1);
    }

    void OnDestroy()
    {
        // 등록된 이벤트 떼기
        if (Manager_Network.Instance == null)
        {
            if (Manager_Ingame.Instance != null) // 아직 인게임 인스턴스가 파괴되지않았을때만
            {
                Manager_Ingame.Instance.e_FakeInput.RemoveListener(m_PlayerInputEvts);
            }
        }
        else
        {
            Manager_Network.Instance.e_PlayerInput.RemoveListener(m_PlayerInputEvts);
            Manager_Network.Instance.e_HeartBeat.RemoveListener(m_PlayerUpdatePosEvts);
            Manager_Network.Instance.e_PlayerHit.RemoveListener(m_PlayerDamageEvts);
            Manager_Network.Instance.e_PlayerStun.RemoveListener(m_PlayerStunEvts);
        }
    }

    IEnumerator Update_FieldOnItem()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        while (true)
        {
            yield return new WaitForSeconds(itemSearchDuration);
            FindOnFieldItem();
        }
    }
    private void Update()
    {
        // 시선 처리
        if (m_MyProfile.Session_ID == ClientProfile.Session_ID)
        {
            if (m_MyProfile.HP <= 0)
                return;
            m_CameraAxis.localRotation = Quaternion.Euler(
                new Vector3(InputManager.m_Player_Input.View_X, InputManager.m_Player_Input.View_Y, 0f));
        }
        else
        {
            m_CameraAxis.localRotation = Quaternion.Euler(new Vector3(m_Output.View_X, m_Output.View_Y, 0f));
        }
    }

    void FixedUpdate()
    {
        Lerp_Position();

        // 인풋에 따른 이동 처리
        Move();

        // 충돌 예지
        if (ClientProfile.Session_ID == m_MyProfile.Session_ID)
            InputManager.m_Pre_Position = Predict_Collide();
    }

    /// <summary>
    /// 플레이어 입력 이벤트
    /// </summary>
    /// <param name="_Profiles"></param>
    void When_Player_Input(User_Profile[] _Profiles)
    {
        for (int index = 0; index < _Profiles.Length; index++)
        {
            if (m_MyProfile.Session_ID == _Profiles[index].Session_ID)
            {
                Update_Profile(_Profiles[index]);
            }
        }
    }

    /// <summary>
    /// 플레이어 위치 갱신 이벤트
    /// </summary>
    /// <param name="_Profiles"></param>
    void When_Player_UpdatePosition(Session_RoundData _round, User_Profile[] _Profiles)
    {
        for (int index = 0; index < _Profiles.Length; index++)
        {
            if (m_MyProfile.Session_ID == _Profiles[index].Session_ID)
            {
                if (m_MyProfile.Session_ID == ClientProfile.Session_ID)
                    continue;

                Update_Profile(_Profiles[index]);
                Set_Lerp_Gap(m_MyProfile.Current_Pos);
            }
        }
    }

    Vector3 pos_gap;
    void Set_Lerp_Gap(Vector3 _target_pos)
    {
        pos_gap = _target_pos - transform.position;
    }
    void Lerp_Position()
    {
        Vector3 temp_gap = pos_gap * 0.1f;
        transform.position += temp_gap;
        pos_gap -= temp_gap;
    }

    /// <summary>
    /// 입력 업데이트<br/>
    /// 트리거의 경우 눌렀을 때, 뗐을 때 이벤트 발동
    /// </summary>
    /// <param name="_input">새로 들어온 입력값</param>
    void Update_Profile(User_Profile _new_profile)
    {
        // 발싸!
        if (m_Output.Fire != _new_profile.User_Input.Fire && _new_profile.User_Input.Fire == true)
            e_Triggered.Invoke("Fire", _new_profile.User_Input.Fire);
        // 킹호작용!
        if (m_Output.Interact != _new_profile.User_Input.Interact && _new_profile.User_Input.Interact == true)         
            e_Triggered.Invoke("Interact", _new_profile.User_Input.Interact);              

        bool debug_tool_change = false;
        if (Manager_Ingame.Instance.m_DebugMode)
        {
            if (_new_profile.User_Input.Tool_1 && _new_profile.Current_Tool != 1)
            { _new_profile.Current_Tool = 1; debug_tool_change = true; }
            else if (_new_profile.User_Input.Tool_2 && _new_profile.Current_Tool != 2)
            { _new_profile.Current_Tool = 2; debug_tool_change = true; }
            else if (_new_profile.User_Input.Tool_3 && _new_profile.Current_Tool != 3)
            { _new_profile.Current_Tool = 3; debug_tool_change = true; }
            else if (_new_profile.User_Input.Tool_4 && _new_profile.Current_Tool != 4)
            { _new_profile.Current_Tool = 4; debug_tool_change = true; }
        }
        if (m_MyProfile.Current_Tool != _new_profile.Current_Tool || debug_tool_change)
        {
            ChangeTool(_new_profile.Current_Tool);
            e_ToolChanged.Invoke(_new_profile.Current_Tool);
        }

        // 특수 기술!
        if (m_MyProfile.m_Using_Skill != _new_profile.m_Using_Skill)
            e_RoleSkill_Toggle.Invoke(_new_profile.m_Using_Skill);

        m_MyProfile = _new_profile;
        if (IsMyCharacter())
            ClientProfile = m_MyProfile;

        m_Output = _new_profile.User_Input;

        if (ClientProfile.User_Input.Interact)
        {
            AcquireItem();
        }
    }

    public bool IsGuard()
    {
#if UNITY_2020_2_NEWER
        return m_MyProfile.Role_Index switch
        {
            1 => false,
            2 => true,
            _ => false,
        };
#else
        switch (m_MyProfile.Role_Index)
        {
            case 1:
                return true;
            case 2:
                return false;
            default:
                return false;
        }
#endif
    }

    public bool IsAlive()
    {
        if (m_MyProfile != null && (m_MyProfile.HP > 0))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 해당 오브젝트에게로 카메라 고정 지시
    /// </summary>
    public void Fix_Camera()
    {
        Update_Render(false);
        Camera.main.transform.SetParent(m_CameraAxis);
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localRotation = Quaternion.identity;
    }

    public void Update_Render(bool _enable)
    {
        if (m_MyProfile.Role_Index == 1)
        {
            if (m_Renderers == null)
            {
                List<Renderer> renderlist = new List<Renderer>();
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                {
                    if (r.gameObject.CompareTag("RenderingPart")) // 렌더 대상인지 체크
                        renderlist.Add(r);
                }
                m_Renderers = renderlist.ToArray();
            }

            foreach (Renderer rd in m_Renderers)
                rd.gameObject.layer = LayerMask.NameToLayer(_enable ? "Player" : "NotRender");
        }
    }

    Vector3 Calculate_Direction()
    {
        // 시점 방향값
        float view_rad = (-m_CameraAxis.rotation.eulerAngles.y) * Mathf.Deg2Rad;
        // 스틱 방향값
        float stick_rad = Mathf.Atan2(m_Output.Move_Y, m_Output.Move_X);
        // 총합 방향값
        float dir_rad = view_rad + stick_rad;

        // 방향 벡터
        return new Vector3(Mathf.Cos(dir_rad), 0f, Mathf.Sin(dir_rad));
    }

    void Move()
    {
        if (m_Is_Stunned)
        {
            Vector3 vec = new Vector3(0f, GetComponent<Rigidbody>().velocity.y, 0f);
            GetComponent<Rigidbody>().velocity = vec;
            return;
        }

        // 만약에 이동하지 않을때
        if (m_Output.Move_X == 0 && m_Output.Move_Y == 0)
        {
            GetComponentInChildren<CapsuleCollider>().sharedMaterial = fullFriction;
        }
        else // 이동중일때
        {
            GetComponentInChildren<CapsuleCollider>().sharedMaterial = noFriction;
        }

        Vector2 dist = new Vector2(m_Output.Move_X, m_Output.Move_Y);
        if (dist.sqrMagnitude <= 0.1f)
            return;
        Vector3 dir = Calculate_Direction();
        Vector3 movement = dir * moveSpeed * 100f * Time.fixedDeltaTime;

        Vector3 vector3 = new Vector3(movement.x, GetComponent<Rigidbody>().velocity.y, movement.z);
        GetComponent<Rigidbody>().velocity = vector3;
    }

    /// <summary>
    /// 충돌 예측
    /// </summary>
    Vector3 Predict_Collide()
    {
        // 무입력시 체크 X
        if (m_Output.Move_X.Equals(0.0f) && m_Output.Move_Y.Equals(0.0f))
            return transform.position;

        // 캐릭터 콜라이더 취득
        //CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        // TODO : 모델을 따라가는 콜라이더를 취득.
        CapsuleCollider collider = gameObject.GetComponentInChildren<CapsuleCollider>();

        // 방향 계산
        Vector3 dir = Calculate_Direction();
        // 예측해야하는 거리 계산
        float dist = moveSpeed * Manager_Ingame.Instance.m_Input_Update_Interval;
        // float dist = collider.radius + moveSpeed * Manager_Ingame.Instance.m_Input_Update_Interval;

        // 충돌 체크 시작
        // Physics.CapsuleCast  ==> 원래 해야하는 충돌체크
        Ray ray = new Ray(transform.position + new Vector3(0f, ASCENDING_LIMIT, 0f) + dir * collider.radius, dir);
        RaycastHit hit;
        Vector3 final_pos;
        int mask = LayerMask.NameToLayer("Player");

        if (Physics.Raycast(ray, out hit, dist, ~mask))
        {
            Vector3 hitpos = hit.point; // 충돌 좌표를 저장
            // Debug.Log("충돌된 좌표 : " + hitpos + " 충돌 오브젝트 타입 : " + hit.collider.ToString());
            m_MyProfile.Current_Pos = hitpos;
            final_pos = hitpos;
        }
        else
            final_pos = ray.GetPoint(dist); // 부딪힌 거 없으면 적당히 나아가기만 하기
        final_pos -= dir * collider.radius;

        // 충돌 체크 시작 (아랫 방향)
        ray = new Ray(final_pos + new Vector3(0f, ASCENDING_LIMIT, 0f), new Vector3(0f, -1f, 0f));
        if (Physics.Raycast(ray, out hit, ASCENDING_LIMIT * 2f, ~mask))
        {
            final_pos = hit.point;
        }
        return final_pos;
    }

    /// <summary>
    /// 툴 매니저로부터 툴 프리팹을 얻어와 생성하고, 그것을 손에 쥐게한다
    /// 생성된 툴은 기본적으로 비활성화되어있다
    /// </summary>
    /// <param name="_oid"></param>
    /// <returns></returns>
    Tool MakeTool(ushort _oid)
    {
        if (_oid == 0)
            return null;

        GameObject prefab = Manager_Tool.Instance.Get_Tool_Prefab(_oid);
        if (prefab == null)
            return null;

        GameObject tool = Instantiate(prefab, m_ToolAxis);
        tool.transform.localPosition = Vector3.zero;
        tool.transform.localRotation = Quaternion.identity;
        tool.SetActive(false);

        return tool.GetComponent<Tool>();
    }

    void ChangeTool(int _index)
    {
        for (int i = 0; i < m_Tools.Length; i++)
        {
            if (m_Tools[i] != null)
            {
                bool enable = i + 1 == _index;
                m_Tools[i].gameObject.SetActive(enable);
                if (enable)
                    m_Tools[i].Register(e_Triggered);
                else
                    m_Tools[i].Unregister();
            }
        }
    }

    void Damage(UInt16 _id, UInt16 _damage)
    {
        Debug.Log(m_MyProfile.Session_ID + "==" + _id + "에게 " + _damage + "의 피해");
        if (m_MyProfile.Session_ID != _id)
            return;

        int hp = m_MyProfile.HP;
        hp = Math.Max(0, hp - _damage);
        m_MyProfile.HP = (UInt16)hp;
        e_Damaged.Invoke(_damage);
    }

    void Stun(UInt16 _id, UInt16 _tick)
    {
        Debug.Log(m_MyProfile.Session_ID + "==" + _id + "에게 " + (_tick / 1000f) + "초 스턴");
        if (m_MyProfile.Session_ID != _id)
            return;

        StartCoroutine(Stun_Process(_tick));
        e_Stunned.Invoke(_tick);
    }
    IEnumerator Stun_Process(UInt16 _tick)
    {
        m_Is_Stunned = true;
        yield return new WaitForSecondsRealtime(_tick / 1000.0f);

        m_Is_Stunned = false;
        yield return null;
    }
    /// <summary>
    /// 내 세션이 맞는지 확인
    /// </summary>
    /// <returns></returns>
    bool IsMyCharacter()
    {
        if(ClientProfile.Session_ID == m_MyProfile.Session_ID)
        {
            return true;
        }
        return false;
    }

  

    /// <summary>
    /// 아이템을 습득합니다.
    /// </summary>
    public void AcquireItem()
    {
        ItemBase item = FindViewInItem(); // 만약 아이템을 발견했다면 해당 아이템을 가져와서 
        
        if (!ReferenceEquals(item, null) && !ReferenceEquals(Manager_Network.Instance, null)) // 통신이 안끊겼고, 아이템일때
        {
            Debug.Log("아이템명칭 : " + item.item.itemName + "," + "반환받은 객체이름 : " + item.name);
            // TODO : 캐릭터가 발견한 아이템정보를 서버에 보내서, 습득 완료 및 캐릭터에 종속시키는 부분이 들어오면 될거같아요.
            Packet_Sender.Send_Item_Get(item.itemData.IID);
            
            TooltipManager.Instance.InvokeTooltip(_msg =>
            {
                _msg.ShowMessage(MessageStyle.ON_SCREEN_UP_MSG, "도둑팀이 " + item.item.itemName + "을/를 습득했습니다.");
            }, MessageStyle.ON_SCREEN_UP_MSG);
        }
    }
 
    /// <summary>
    /// 아이템을 탐색합니다.
    /// </summary>
    /// <returns></returns>
    ItemBase FindViewInItem()
    {
        if (IsMyCharacter())
        {
            Collider[] colliders = Physics.OverlapSphere(m_MyProfile.Current_Pos, acquireDist, ItemLayer.value); // O자형태로, 탐색거리만큼 아이템 콜라이더 취득
            int col_length = colliders.Length;

            hitPos = new Vector3[col_length];
            dir    = new Vector3[col_length];

            for (int i = 0; i < col_length; i++)
            {
                hitPos[i] = colliders[i].transform.position;
                dir[i] = (hitPos[i] - m_MyProfile.Current_Pos).normalized;

                if (dir[i].sqrMagnitude < acquireDist) // 범위안이면
                {
                    IsHit = true;
                    ItemBase temp = colliders[i].transform.GetComponentInChildren<ItemBase>();
                    if (temp.itemData.IID > 0)
                    {
                        return temp;
                    }
                }
                else
                {
                    IsHit = false;
                }
            }
        }

        return null;
    }

    ItemBase FindViewInNoPressItem()
    {
        if (IsMyCharacter())
        {
            Collider[] colliders = Physics.OverlapSphere(m_MyProfile.Current_Pos, acquireDist, ItemLayer.value); // O자형태로, 탐색거리만큼 아이템 콜라이더 취득
            int col_length = colliders.Length;

            hitPos = new Vector3[col_length];
            dir    = new Vector3[col_length];

            for (int i = 0; i < col_length; i++)
            {
                hitPos[i] = colliders[i].transform.position;
                dir[i] = (hitPos[i] - m_MyProfile.Current_Pos).normalized;

                if (dir[i].sqrMagnitude < acquireDist) // 범위안이면
                {                   
                    // 중간 장애물이 없을때
                    if (Physics.Raycast(m_MyProfile.Current_Pos, dir[i], out RaycastHit hitinfo, acquireDist, ItemLayer.value))
                    {
                        /*Debug.Log("템 주움::캐릭터위치->히트정보 포인트");
                        Debug.DrawRay(m_MyProfile.Current_Pos, hitinfo.point, Color.red);
                        Debug.Log("템 주움::캐릭터위치->방향 포인트");
                        Debug.DrawRay(m_MyProfile.Current_Pos, dir[i], Color.yellow);*/

                        IsHit = true;
                        return hitinfo.collider.GetComponentInChildren<ItemBase>();
                    }
                    else
                    {
                        //IsHit = false;
                    }

                }
            }
        }
        return null;
    }

    void FindOnFieldItem()
    {
        ItemBase item = FindViewInNoPressItem();
        if (item != null)
        {
            TooltipManager.Instance.InvokeTooltip(x =>
            {
                x.ViewSideInItemMessage(item, m_MyProfile.Current_Pos, acquireDist);
                x.DrawOnHeadMessage(m_CameraAxis.gameObject);
                x.ShowMessage(MessageStyle.ON_HEAD_MSG, item.item.itemName + "아이템을 찾음 ㅅㄱ", m_CameraAxis.position);
            }, MessageStyle.ON_HEAD_MSG);
        }
        else
        {
            TooltipManager.Instance.InvokeTooltip(x =>
            {
                x.ShowMessage(MessageStyle.ON_HEAD_MSG, "아이템을 못찾았어!", m_CameraAxis.position);
            }, MessageStyle.ON_HEAD_MSG);
        }
    }

#if UNITY_EDITOR
     private void OnDrawGizmosSelected()
     {
        Handles.color = IsHit ? Color.cyan : Color.green;
        Handles.DrawSolidDisc(m_MyProfile.Current_Pos, Vector3.up, acquireDist);               
     }
#endif

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < hitPos.Length; i++)
        {
            Handles.color = IsHit ? Color.red : Color.green;
            Handles.DrawLine(m_MyProfile.Current_Pos, hitPos[i]);
        }
    }
#endif
}
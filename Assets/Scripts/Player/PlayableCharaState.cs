/// <summary>
/// 플레이어블 캐릭터의 상태 열거형
/// 언더바는 몬스터러브 스테이트머신의 구조상 인식에 방해가 되니(이벤트 식별자로 오인함) 언더바를 쓰지 않습니다.
/// </summary>
public enum PlayableCharaState
{
    PlayerIdle,
    PlayerMove,
    PlayerOnHit,
    PlayerOnStun,
    PlayerOnDeath,
    PlayerOnCloaking
}

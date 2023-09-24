# MMO_JSRPG
C#서버와 유니티로 만든 2D 격자 MMORPG 프로젝트

## 기간
* 2023년 7월 4일 ~ 9월 3일: 클라이언트, 서버 연동
* 2023년 9월 4일 ~ 9월 24일: 데이터베이스 추가, 로그인 화면, 서버 선택 화면, 서버 구조 개선

## 참고자료
* 프로젝트를 진행하며 정리, 공부한 내용과 생각은 **[Velog - Seoki.log](https://velog.io/@pkoi5088)** 에서 볼 수 있습니다.
* 블로그의 GameTag나 ServerTag를 확인하면 쉽게 찾을 수 있습니다.

## 환경
* .NET6.0
* Unity 2022.3.3f1
* ASP.NET
* Microsoft Sql Server

## 설치 Nuget패키지
* Google.Protobuf - 3.24.2
* Microsoft.EntityFrameworkCore.SqlServer - 7.0.10
* Microsoft.EntityFrameworkCore.Tools - 7.0.10
* Newtonsoft.json - 13.0.3

## 서버구조
<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/f5e7de9c-1d83-4d00-8409-66cc52fc4205/image.png" width="70%" height="70%"></p>

1. **Web Server**  
ASP.NET Core로 로그인, 회원가입을 위한 서버를 구축, 로그인에 성공한 사용자정보는 공유DB에 업로드해 게임서버로 접근하는 클라이언트의 식별정보를 제공한다. 로그인 확인 응답을 받은 클라이언트는 게임서버에 연결을 요청한다.

2. **Game Server**  
.NET 프레임워크에서 제공하는 Socket기반 비동기식 통신을 구현한 뒤, Google Protobuf 적용해 proto 파일에서 정의한 패킷의 Handler를 구현하는 방식으로 서버를 설계하였다. 사용한 DBMS는 .NET에서 제공하는 Sql Server로 EntityFramework.Core를 사용하였다.

### 구현 기능
- 사용자 로그인
- 게임입장
- 게임퇴장
- 플레이어 생성
- 플레이어 이동
- 플레이어 공격 및 체력변화
- 아이템 장착 및 인벤토리, 스텟창

> **기능을 구현하면서 공부하고 학습한 내용은 [Velog - Seoki.log](https://velog.io/@pkoi5088?tag=game)에서 확인할 수 있습니다.**

## 조작방법
* 이동 - WASD
* 공격 - SPACE

## 실행화면
1. **로그인**
<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/8c26a953-2848-45aa-8186-914b40bbef39/image.png" width="70%" height="70%"></p>

2. **서버선택**
<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/cc772aed-6345-4981-b199-0a2fb9b76be5/image.png" width="70%" height="70%"></p>

3. **인게임(2 Player)**
<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/241a764e-5cdc-4840-bb0c-1ad80584c1c7/image.png" width="100%" height="100%"></p>

<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/c156538d-7bb5-4d14-9afa-3017cc4c4a06/image.png" width="100%" height="100%"></p>

<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/f82e48c2-564c-4d8e-82e1-22ad3b453445/image.png" width="100%" height="100%"></p>
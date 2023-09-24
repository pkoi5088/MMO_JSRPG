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

## 설치 Nuget패키지
* Google.Protobuf - 3.24.2
* Microsoft.EntityFrameworkCore.SqlServer - 7.0.10
* Microsoft.EntityFrameworkCore.Tools - 7.0.10
* Newtonsoft.json - 13.0.3

## 실행방법
1. Server 솔루션의 Server 프로젝트와 AccountServer 프로젝트가 동시에 작동하도록 설정해주세요.
2. 두 개의 프로젝트가 모두 켜지면 서버가 정상적으로 켜진겁니다.
3. Client 디렉토리는 Unity Project로 해당 버전의 유니티로 실행시킵니다.
4. 상단바의 Tools -> Run Multiplayer -> Player로 동시에 실행할 Player를 지정하면 게임이 실행됩니다.

## 조작방법
* 이동 - WASD
* 공격 - SPACE

## 실행화면

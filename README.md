# MMO_JSRPG
C#서버와 유니티 MMORPG 프로젝트

## 기간
* 2023년 7월 4일 ~ 2023년 9월 3일: 클라이언트, 서버 연동
* 2023년 9월 4일 ~ NOW: 데이터베이스 추가, 서버 구조 개선

## 참고자료
* 강의를 들으며 정리한 내용과 나의 생각은 **[Velog - Seoki.log](https://velog.io/@pkoi5088)** 에서 볼 수 있습니다.
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
1. Server 솔루션을 실행하면 서버가 켜집니다.
2. Client 디렉토리는 Unity Project로 해당 버전의 유니티로 실행시킵니다.
3. 상단바의 Tools -> Run Multiplayer -> Player로 동시에 실행할 Player를 지정하면 게임이 실행됩니다.

## 조작방법
* 이동 - WASD
* 공격 - SPACE

## 실행화면
<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/d99ae87a-3bce-4279-83a2-df3af5fef377/image.png" width="100%"></p>

<p align="center"><img src="https://velog.velcdn.com/images/pkoi5088/post/27d708f1-a467-4146-9f0c-5fa92aecd9ff/image.png" width="100%"></p>

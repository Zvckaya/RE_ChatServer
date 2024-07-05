# RE-ChatServer 구현

### 서버코어

- 챗 세션
- 커넷터
- 리스너
- 룸
- 세션

### 서버

- 메인 프로그램

### 클라이언트

- 메인 프로그램

서버코어 - 실제 사용되는 서버 엔진 라이브러리 

서버 - 라이브러리를 이용한 리슨 서버 

클라이언트 - 송/수신용 클라이언트 

## 구조

# 서버

가장 기초적으로 세션들을 감시할 리스너를 구동하고 있어야 한다 .

또한 이 리스너는 서버에서 사용할 세션클래스로 매핑되어 있어야함 

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/0e39d2e1-fbd8-4eba-9792-60dd170987b9/Untitled.png)

리스너생성 및 서버에서 사용할 세션 타입을 Func을 이용하여 넘기고 있다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/2cfbe206-4e16-42f3-8c22-726dffba0557/Untitled.png)

OnConnected,OnDisconnected,OnRecv,OnSend등의 관리 메소드를 이용하여 이벤트가 완료 된 시점에 일어날 이벤트들을 정의해주었다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/adbdf513-31ec-49cb-a8ec-a334cd477044/Untitled.png)

# 리스너의 Init

먼저 엔드포인트 및 생성할 세션 타입을 인자로 받아준다 .

이를 이용한 리슨 소켓을 만든 후 , BInding작업과 최대 listen할 대기열을 만들어준다.

또한 accept이 비동기로 완료되었을때 일어날 이벤트를 관리할 소켓 비동기 이벤트도 만들어준다 .

 

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/215af342-3752-48c5-818a-bb2ed0296762/Untitled.png)

### RegisterAccept 및 OnAcceptCompleted

레지스터에선 AcceptAysnc를 이용하여 비동기로 접속을 진행한다. 

대기가 있으면 기다린 후 , 완료되면 콜백으로 OnAcceptComplete를 호출하여 
세션 고유 id를 발급하고 , 실제 세션을 생성하여 발급한다 .

또한 Room 단위로 세션들을 관리하고 있기 때문에 , 룸 매니저에 발급한 세션을 추가해준다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/31f90746-1ff0-40e9-baa6-006bec0f5918/Untitled.png)

# Session

세션클래스는 기본적으로 가상 함수로 구현되야 한다.

클라이언트나 서버에서의 세션 처리를 구분하기 위함이다.

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/2c0bd1dc-00a1-45d4-88b8-f407476fb5cc/Untitled.png)

### Start

스타트 메소드에서는 리스너에서 발급된 소켓을 받아. 매핑후 , 

전송과 수신에 사용할 소켓비동기 이벤트를 각각 선언및 콜백 매핑해준다 .

또한 컨텐츠 상에서 사용할 추상 함수인 OnConnect는 이때 선언된다 .

실행과 동시에, 세션은 수신대기상태가 되어야 할 것이므로 RegisterRecv를 호출한다. 

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/6d582569-68d0-4c7c-911b-78811093e20c/Untitled.png)

### Disconnect

일단 나의 세션이 종료 되었는지 확인해주어야한다→ 종료후에도 또 종료를 호출 하면 
NullException오류가 발생한다. 확인후 컨텐츠에서의 종료 액션매핑을 위해 OnDIsconnected이 호출되어야 할것이다. 룸 매니저에서도 삭제해준다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/391cec8c-11af-45c3-b603-497c066e5d2e/Untitled.png)

### RegisterRecv및 OnRecvCompleted

비동기 소켓 메소드를 이용하여 수신 감지를 진행한다. 

대기 후, 콜백 메소드인 OnRecvCOmpleted가 호출되어 ,문제 없을시 

비동기 이벤트의 버퍼를 ArraySegment로 만들어 컨텐츠 딴에서 사용되는 OnRecv에 전달해준 후, 

다시 수신 감지 모드로 돌아간다 . 그 이외의 경우에는 접속 종료해준다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/dc6a778d-da63-4a3f-9fee-2e1ced27faf6/Untitled.png)

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/fa0b7272-44d5-4070-94ce-1bed908eea3d/Untitled.png)

### Send 및 OnSendCompleted

여러 스레드가 동시에 어떠한 세션의 Send를 요청할 수 있기 때문에 , 

Send에는 lock을 사용하는것이 바람직하다 .

먼저 버퍼를 설정해준 후 전송이 확인되면 콜백을 호출해준다 .

보내고 딱히 할 것은 없고. OnSend는 이때 호출 되어야 할 것이다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/b8082788-a30d-4b56-be06-4152df23a6c7/Untitled.png)

# Room

세션들은 룸이라는 형태로 관리된다 .

룸마스터 싱글톤을 통해 각각의 방들을 관리할 수 있고 여기서 BroadCasting 또한 진행한다 .

### Enter

세션을 받아 룸마스터에 추가해준다 .

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/93efe70d-ba67-420b-83d9-7822e5174224/Untitled.png)

### BroadCast

브로드 캐스트는 string을 인자로 받아 

룸마스터에 있는 모든 세션에 Send해준다. (이때 단독 사용을 위해 lock을 선언해주어야한다.)

# Connector

커넥터는 클라이언트가 서버에 비동기 방식을 사용하여 연결 요청을 보내고 연결이 완료되면 후속 작업을 처리하기 위해 구현한다(동기성 구현)

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/d74a27e5-3e7c-426b-98d8-29ac71898b73/5db9c5da-c865-4003-bd15-0610f67ed599/Untitled.png)

### OnConnectedCompleted

ConnectAsync가 완료되면 콜백으로 호출되며 , 이 과정에서 클라이언트 측에서 사용할 세션이 발급된다 .

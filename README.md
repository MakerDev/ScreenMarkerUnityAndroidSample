ScreenMarker 안드로이드 사용 가이드
=============
# 1. 안드로이드 스튜디오에서 코드 대체하기
제공된 ScreenMarker.java 파일의 내용으로 원본 프로젝트의 ScreenMarker.java의 파일의 내용을 덮어씁니다. 이때, 파일 최상단의 패키지명은 원본 프로젝트의 패키지명으로 변경해야 합니다. //TODO 주석을 통해 변경해야 할 부분을 확인할 수 있습니다.

업데이트를 완료한 후에는 평소처럼 빌드하여 aar파일을 생성합니다. 해당 파일을 추후 유니티 프로젝트에 추가하여 사용합니다. 유니티 샘플프로젝트와 함께 제공된 plugin-debug.aar은 테스트용으로 디버그 빌드로 생성되었습니다.


# 2. 유니티 연동
## 2.1. aar 파일 추가
유니티 프로젝트의 Assets/Plugins/Android 폴더에 aar 파일을 추가합니다. 파일명은 무엇으로 하여도 특별히 상관은 없습니다. 예시 프로젝트에는 plugin-debug.aar로 추가되어 있습니다.

## 2.2. PlayerSettings 설정
유니티 프로젝트의 PlayerSettings에서 Minimum API Level을 28로 설정합니다.
기본값이 상당히 낮게 설정되어 있을텐데, 이 경우 AndroidManifest.xml 관련 오류가 발생할 수 있습니다.

![PlayerSettings](/Images/5_PlayerSetting.png)

![미변경시 발생하는 오류](/Images/4_Manifest%20Build%20Failure.png)


## 2.3. C# 스크립트 작성
테스트 스크립트가 Assets/Scripts/ScreenMarkerAndroidPlugin.cs에 작성되어 있습니다. 해당 스크립트를 참고하여 사용하시면 됩니다. 기존에 유니티<->안드로이드 연동방식과 특별히 달라지는 것은 없으며 AndroidJavaObject를 통해 ScreenMarker 클래스를 호출하는 방식입니다. 다만, 클래스 안의 package name 관련 변수의 값은 사용하시는 패키지 명으로 변경해주셔야합니다. //TODO 주석을 통해 변경해야 할 부분을 확인할 수 있습니다.

사용시 주의할 점등은 주석을 통해 확인할 수 있습니다. 가장 주의하셔야할 부분은
안드로이드 api 호출 시 ui 스레드에서 호출하도록 하는 것과 float 값을 int로 넘기지 않도록 하는 것입니다. 자세한 내용은 주석을 참고해주시기 바랍니다.

![패키지 변수 명 변경](/Images/1_Android%20Plugin%20PackageName.png)

## 2.4. 이미지 에셋 추가
테스트에 사용하는 이미지는 Assets/Sprites 폴더에 추가되어있습니다. 직접 추가하실때는 아래와 같이 타입을 Sprite(2D and UI)로 설정해주셔야하며, Read/Write Enabled를 체크해주셔야합니다.

![이미지 에셋 설정](/Images/0_Unity%20Image%20Setting.png)

## 2.5. 씬에 플러그인 추가
씬에 빈 게임오브젝트를 만드시고 앞서 만든 ScreenMarkerAndroidPlugin.cs 스크립트를 추가합니다. 해당 스크립트는 이미지를 로드하도록 되어 있으니, 앞서 추가한 스프라이트를 연결해주시면 됩니다.

![게임오브젝트 추가](/Images/3_Plugin%20Gameobject.png "게임오브젝트 추가")

## 2.6. 버튼 추가
버튼을 추가하고, 버튼의 OnClick 이벤트에 앞서 추가한 게임오브젝트의 스크립트를 연결합니다. 예시 프로젝트에 일부 기능들만 구현되어 있습니다만 다른 모든 api도 같은 방식으로 유니티와 연동 가능합니다. (추가예정)

현재는 ShowScreenMarker버튼과 PrintTiledTextAndImage 버튼, 그리고 Hide, Reset버튼이 존재합니다.

## 2.7. 빌드
안드로이드로 빌드하여 실행하시면 됩니다. 아래는 Print Tiled Text and Image 버튼을 눌렀을 때의 예시입니다.

<img src="Images/2. PrintTile Example.jpg" width="300px">
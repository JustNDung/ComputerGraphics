# Computer Graphics

## Tổng quan

`Computer Graphics` là một dự án Unity phát triển các thí nghiệm giáo dục về cơ học. Dự án mô phỏng đòn bẩy, momen lực, tác động của lực và các bài toán thực tế thông qua một hệ thống nhiệm vụ/quest, phần thưởng/reward và cảnh chuyển động.

## Mục tiêu dự án

- Dạy người dùng khái niệm đòn bẩy và momen lực.
- Xây dựng hệ thống quest linh hoạt dễ mở rộng.
- Tách riêng logic gameplay, vật lý và UI.
- Cho phép dễ dàng thêm thí nghiệm mới bằng ScriptableObject.

## Phần chính của hệ thống

### Quest & Objective

- `Assets/Scripts/Quest/QuestManager.cs`
  - Quản lý danh sách `activeQuests` và khởi tạo từ `startQuests`.
  - Xử lý sự kiện `RewardEvent` và kích hoạt `CompleteQuest` khi nhiệm vụ hoàn tất.

- `Assets/Scripts/Quest/QuestSO.cs`
  - ScriptableObject chứa các thông tin nhiệm vụ:
    - `questId`, `title`, `description`.
    - `zoneType` để xác định vùng tương tác liên quan.
    - `objectives`: danh sách `ObjectiveSO`.
    - `completionReward`: phần thưởng hoàn thành.

- `Assets/Scripts/Quest/ObjectiveSO.cs`
  - Abstract ScriptableObject cho phép định nghĩa nhiều loại mục tiêu khác nhau.
  - Phương thức `Match(RewardEvent e)` so khớp event với điều kiện.

### Reward System

- `Assets/Scripts/Reward/RewardSystem.cs`
  - Singleton xử lý các `RewardEvent` và áp dụng các `RewardRuleSO` phù hợp.
  - Lưu tổng `knowledgePoints` và in log reward.

- `Assets/Scripts/Reward/RewardRuleSO.cs`
  - Mỗi rule xác định điều kiện bằng `Evaluate(RewardEvent e)`.
  - `GetReward(RewardEvent e)` trả về `RewardData` chứa điểm và thông báo.

- `Assets/Scripts/Reward/RewardEvent.cs`
  - Event trung gian sử dụng:
    - `experimentId` (định danh thí nghiệm)
    - `actionId` (định danh hành động)
    - `success` (kết quả hành động)

- `Assets/Scripts/Reward/RewardData.cs`
  - Chứa `knowledgePoints` và `message` để hiển thị.

### Event / Message Dispatcher

- `Assets/Scripts/MessageDispatcher/MessageDispatcher.cs`
  - Event bus kiểu `Type -> Action<IMessage>`.
  - Hỗ trợ `Subscribe<T>`, `Unsubscribe<T>`, `Publish<T>`.
  - Giúp truyền message giữa trigger, UI và gameplay mà không cần phụ thuộc trực tiếp.

- `Assets/Scripts/TriggerZone/TriggerZone.cs`
  - Gửi `TriggerZoneEnterMessage` và `TriggerZoneExitMessage` khi player vào/ra vùng.

### Scene Management

- `Assets/Scripts/Scene/SceneLoader.cs`
  - Singleton `SceneLoader.Instance` với `DontDestroyOnLoad`.
  - Quản lý queue load cảnh để tránh load chồng chéo.
  - Hỗ trợ fade in/out, loading screen và progress bar.
  - Dùng `AsyncOperation` và các coroutine để nạp cảnh bất đồng bộ.

- `Assets/Scripts/Scene/SceneDatabase.cs`
  - Ánh xạ `SceneID` sang tên cảnh thực tế.

- `Assets/Scripts/Scene/SceneID.cs`
  - Định nghĩa các ID cảnh: `Bootstrap`, `MainMenu`, `Gameplay`, `Loading`, `Test`.

## Thuật toán và mô hình nổi bật

### Mô phỏng đòn bẩy

- `Assets/Scripts/LeverController.cs`
  - Tính momen lực:
    - `M = F × d`
    - `torque1 = forceF1 * armLength1`
    - `torque2 = forceF2 * armLength2`
    - `netTorque = torque1 - torque2`
  - Áp dụng `Rigidbody.AddTorque(Vector3.forward * (-netTorque * 0.1f))`.
  - Kiểm tra cân bằng với `Mathf.Approximately(M1, M2)`.
  - Vẽ Gizmos lực để debug và trực quan hóa.

### Điều khiển cửa

- `Assets/Scripts/Door/DoorController.cs`
  - Sử dụng `OnTriggerEnter`/`OnTriggerExit` để phát hiện player.
  - Người chơi nhấn phím `F` để mở cửa khi ở trong vùng.
  - Cập nhật trạng thái animator `isOpen`.

### Hệ thống quest/reward dựa trên sự kiện

- `QuestManager` và `RewardSystem` dùng cùng một kiểu dữ liệu `RewardEvent`.
- `RewardRuleSO` và `ObjectiveSO` tương tác thông qua `experimentId` và `actionId`.
- Chiến lược này giúp mở rộng thí nghiệm mới mà chỉ cần thêm rule/objective mới.

## Thí nghiệm/Quest đã triển khai

### DoorExperiment

- `Quest/SpecificQuests/DoorExperiment/DoorQuest.cs`
- Reward rule: `Reward/SpecificRewards/DoorExperiment/DoorCompleteReward.cs`
  - Điều kiện: `experimentId == "door" && success == true`
  - Điểm thưởng: `50 KP`

### WrenchExperiment

- `Quest/SpecificQuests/WrenchExperiment/WrenchQuest.cs`
- Objective mẫu: `WrenchCompareMomentObjective`
  - Điều kiện: `experimentId == "wrench" && actionId == "compare_correct"`
- Reward rule: `WrenchCompareMomentReward`
  - Điểm thưởng: `60 KP`

### Lever / RealWorldLever

- Các rule `Reward/SpecificRewards/Lever/` và `Reward/SpecificRewards/RealWorldLever/` kiểm tra đúng loại đòn bẩy và ví dụ đời thực.
- Hệ thống này phù hợp cho bài kiểm tra nhận diện dụng cụ và phân loại loại đòn bẩy.

## Dữ liệu ScriptableObject

- Tất cả quest và reward rule đều định nghĩa dưới dạng ScriptableObject.
- Tăng khả năng cấu hình trong Unity Editor mà không cần sửa code.
- Mở rộng bằng cách tạo class mới và asset trong Editor.

## Packages & Công nghệ sử dụng

- Unity Engine
- `com.unity.inputsystem`
- `com.unity.cinemachine`
- `com.unity.ai.navigation`
- `com.unity.probuilder`
- `com.unity.render-pipelines.universal`
- `com.unity.ugui`
- `com.unity.visualscripting`
- `com.unity.cloud.gltfast`

## Cấu trúc thư mục quan trọng

- `Assets/Scripts/Quest/`
- `Assets/Scripts/Reward/`
- `Assets/Scripts/TriggerZone/`
- `Assets/Scripts/Scene/`
- `Assets/Scripts/Door/`
- `Assets/Scripts/UI/`
- `Assets/Scripts/LeverController.cs`

## Đề xuất cải tiến

- Hoàn thiện `UIManager` để hiện reward và trạng thái quest.
- Thêm UI cụ thể cho từng thí nghiệm dựa trên `ExperimentUI`.
- Lưu trạng thái quest, điểm KP giữa các cảnh.
- Bổ sung tính tương tác vật lý đòn bẩy đa dạng hơn.
- Thêm điều kiện success/failure rõ ràng cho từng quest.

---

README này cung cấp cái nhìn chi tiết hơn về các chức năng, kiến trúc dữ liệu và hệ thống thuật toán mà dự án hiện có.
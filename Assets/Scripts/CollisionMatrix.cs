using System.Collections.Generic;
public static class CollisionMatrix {
    private static bool[,] collisionMatrix = new bool[Config.Instance.OBJECT_COUNT, Config.Instance.OBJECT_COUNT];
    private static Dictionary<int, int> nameMap = new Dictionary<int, int>();
    private static int nameCounter = 0;
    private static int dirtyEnterID = -1; 
    private static int dirtyExitID = -1;
    public static bool isColliding = false;

    public static void RecordCollisionEnter(int id) {
        int key = CheckName(id);

        if (dirtyEnterID == -1) {
            dirtyEnterID = key;
        } else {
            collisionMatrix[key, dirtyEnterID] = true;
            collisionMatrix[dirtyEnterID, key] = true;

            dirtyEnterID = -1;
        }
    }

    public static void RecordCollisionExit(int id) {
        int key = CheckName(id);

        if (dirtyExitID == -1) {
            dirtyExitID = key;
        } else {
            collisionMatrix[key, dirtyExitID] = true;
            collisionMatrix[dirtyExitID, key] = true;

            dirtyExitID = -1;
        }
    }

    public static bool IsColliding(int id) {
        int key = CheckName(id);
        for (int i = 0; i < Config.Instance.OBJECT_COUNT; i++) {
            if (collisionMatrix[i, key]) return true; 
        }
        return false;
    }

    private static int CheckName(int name) {
        if (!nameMap.ContainsKey(name)) {
            nameMap.Add(name, nameCounter);
            return nameCounter++;
        } else {
            int key = -1;
            nameMap.TryGetValue(name, out key);
            return key;
        }
    }
}
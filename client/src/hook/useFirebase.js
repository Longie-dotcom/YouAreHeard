import { getToken } from "firebase/messaging";
import { messaging } from "./firebase";

function useFirebase() {
    const firebase = async () => {
        try {
            const permission = await Notification.requestPermission();
            if (permission !== "granted") return null;

            const token = await getToken(messaging, {
                vapidKey: "BIL6vUrnN4tKnJjJtRjVaZS71vpGRQFooeVUK2G1oreCf5rzbX4X4dMU3HDBLUQ0RBEdV9Dmy_qyZWVtH_6FoO4"
            });

            return token;
        } catch (err) {
            return null;
        }
    }

    return { firebase };
}

export default useFirebase;

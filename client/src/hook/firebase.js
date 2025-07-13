// firebase.js
import { initializeApp } from "firebase/app";
import { getMessaging } from "firebase/messaging";

const firebaseConfig = {
  apiKey: "AIzaSyBSdsrMo65H9E--WiQPuMhcBJxRSXIg_bk",
  authDomain: "pill-reminder-d5663.firebaseapp.com",
  projectId: "pill-reminder-d5663",
  storageBucket: "pill-reminder-d5663.firebasestorage.app",
  messagingSenderId: "591683259599",
  appId: "1:591683259599:web:370eb2aceb0f899949bbba",
  measurementId: "G-3BX6FXXKLV"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Cloud Messaging
const messaging = getMessaging(app);

export { messaging };

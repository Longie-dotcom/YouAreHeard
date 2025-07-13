importScripts('https://www.gstatic.com/firebasejs/10.12.2/firebase-app-compat.js');
importScripts('https://www.gstatic.com/firebasejs/10.12.2/firebase-messaging-compat.js');

firebase.initializeApp({
  apiKey: "AIzaSyBSdsrMo65H9E--WiQPuMhcBJxRSXIg_bk",
  authDomain: "pill-reminder-d5663.firebaseapp.com",
  projectId: "pill-reminder-d5663",
  storageBucket: "pill-reminder-d5663.firebasestorage.app",
  messagingSenderId: "591683259599",
  appId: "1:591683259599:web:370eb2aceb0f899949bbba",
  measurementId: "G-3BX6FXXKLV"
});

const messaging = firebase.messaging();

messaging.onBackgroundMessage(function(payload) {
  console.log('[firebase-messaging-sw.js] Received background message ', payload);
  const { title, body } = payload.notification;

  self.registration.showNotification(title, {
    body,
    icon: '/icon-192x192.png', // optional icon path
  });
});

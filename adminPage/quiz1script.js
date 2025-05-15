'use strict';

const quizData = [
    {
        question: " What should you do to prepare your space before using the Meta Quest 3 for a VR session? ",
        a: "Remove obstacles and ensure good lighting",
        b: "Close your eyes and sit still",
        c: "Use VR near glass windows",
        d: "Turn off all lights",
        correct: "a",
    },

    {
        question: " What is the recommended usage time for beginners before taking a break to avoid VR fatigue?",
        a: "15 minutes",
        b: "45 minutes",
        c: "30 minutes with a 15-minute break",
        d: "60 minutes nonstop",
        correct: "c",
    },

    {
        question: "What does the IPD setting on the Meta Quest 3 adjust?",
        a: "The brightness of the screen",
        b: "The volume level",
        c: "The distance between the lenses to match your eyes",
        d: "The battery level",
        correct: "c",
    },

    {
        question: "Which button should you press to recenter your view in VR if the alignment feels off?",
        a: "A-button",
        b: "X-button",
        c: "Trigger-button",
        d: "Meta-button (hold)",
        correct: "d",
    },

    {
        question: "What is the purpose of the Guardian system in the Meta Quest 3?",
        a: "To protect your account from hacking",
        b: "To set up boundaries and avoid collisions while in VR",
        c: "To limit the brightness of the screen",
        d: "To reset the device to factory settings",
        correct: "b",
    },

    {
        question: "Which controllers' buttons are typically used to perform actions like selecting or shooting in a game?",
        a: "Meta and Menu buttons",
        b: "Trigger and Grip buttons",
        c: "Joystick only",
        d: "X and Y buttons",
        correct: "b",
    },

    {
        question: "Why should you avoid using alcohol-based wipes when cleaning the Meta Quest 3 headset?",
        a: "They won't clean thoroughly",
        b: "They can permanently damage the headset",
        c: "They smell bad",
        d: "They make the headset too slippery",
        correct: "b",
    },
]

const quiz = document.getElementById('quiz');
const answerEls = document.querySelectorAll('.answer');
const questionEl = document.getElementById('question');
const a_text = document.getElementById('a_text');
const b_text = document.getElementById('b_text');
const c_text = document.getElementById('c_text');
const d_text = document.getElementById('d_text');
const submitBtn = document.getElementById('submit');

let currentQuiz = 0;
let score = 0;

loadQuiz()

function loadQuiz(){

    deselectAnswers()

    const currentQuizData = quizData[currentQuiz]

    questionEl.innerText = currentQuizData.question
    a_text.innerText = currentQuizData.a
    b_text.innerText = currentQuizData.b
    c_text.innerText = currentQuizData.c
    d_text.innerText = currentQuizData.d
}

function deselectAnswers () {
    answerEls.forEach(answerEl => answerEl.checked = false)
}

function getSelected () {
    let answer
    answerEls.forEach(answerEl => {
        if(answerEl.checked) {
            answer = answerEl.id
        }
    })
    return answer;
}

submitBtn.addEventListener('click', function () {
    const answer = getSelected()
    if (answer) {
        if (answer === quizData[currentQuiz].correct) {
            score++
        }

        currentQuiz++ 

       if (currentQuiz < quizData.length) {
    loadQuiz();
    } else {
        // Build the data to send
        const scoreData = {
            gameName: "Test",        // Replace with actual game name if dynamic
            playerName: "LeeroyJankins",     // Replace with actual player name (maybe from a login system)
            Score: Math.round((score / 7) * 100)               // The user's final score
        };

        // Send POST request to backend
        fetch(window.env.API_URL+"/api/score/submit", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(scoreData)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(data => {
            console.log(data.message);
            quiz.innerHTML = `
                <h2>You answered ${score}/${quizData.length} questions correctly</h2>
                <button onclick="window.close()">Return</button>
            `;
        })
        .catch(error => {
            console.error("Error submitting score:", error);
            quiz.innerHTML = `
                <h2>You answered ${score}/${quizData.length} questions correctly</h2>
                <p style="color: red;">Failed to submit score.</p>
                <button onclick="window.close()">Return</button>
            `;
        });
    }
    
    }
})
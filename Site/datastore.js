
// let user = {
// 	id: 0,
// 	name: "Sérgio",
// 	number: "+351 917 826 162",
// 	pic: "images/sergio1.jpg"
// };

let user = {
	id: 0,
	name: profile.user.firstName + " " + profile.user.lastName,
	number: profile.user.username,
	pic: "images/dsaad212312aGEA12ew.png"
};


let contactList = profile.contactList;
let groupList = profile.rooms;
let messages = msgSync;
console.log(messages);
/*
let contactList = [
	{
		id: 0,
		name: "Sérgio",
		number: "+351 917 826 162 ",
		pic: "images/sergio1.png",
		lastSeen: "Apr 29 2018 17:58:02"
	},
	{
		id: 1,
		name: "Magno",
		number: "+351 918 232 372",
		pic: "images/alex1.jpg",
		lastSeen: "Apr 28 2018 22:18:21"
	},
	{
		id: 2,
		name: "Alex",
		number: "+351 919 726 312",
		pic: "images/alex2.jpg",
		lastSeen: "Apr 28 2018 19:23:16"
	},
	{
		id: 3,
		name: "Rochinha",
		number: "+351 919 823 263",
		pic: "images/diogo1.jpg",
		lastSeen: "Apr 29 2018 11:16:42"
	},
	{
		id: 4,
		name: "Diogo",
		number: "+351 917 278 138",
		pic: "images/diogo2.jpg",
		lastSeen: "Apr 27 2018 17:28:10"
	}
];
*/


/*
let groupList = [
	{
		id: 1,
		name: "Grupo CD/VC",
		members: [0, 1, 3],
		pic: "images/estipca.jpg"
	},
	{
		id: 2,
		name: "Grupo dos Gajos Programadores",
		members: [0, 2],
		pic: "images/gajosqueprograma.png"
	},
	{
		id: 3,
		name: "Grupo de gajos fixes",
		members: [0],
		pic: "images/sergio2.jpg"
	}
];
*/


// message status - 0:sent, 1:delivered, 2:read

/*
let messages = [
	{
		id: 0,
		sender: 2,
		body: "Onde andas, mossu?",
		time: "April 25, 2018 13:21:03",
		status: 2,
		recvId: 0,
		recvIsGroup: false
	},
	{
		id: 1,
		sender: 0,
		body: "em casa",
		time: "April 25, 2018 13:22:03",
		status: 2,
		recvId: 2,
		recvIsGroup: false
	},
	{
		id: 2,
		sender: 0,
		body: "tás fixe?",
		time: "April 25, 2018 18:15:23",
		status: 2,
		recvId: 3,
		recvIsGroup: false
	},
	{
		id: 3,
		sender: 3,
		body: "tá tudo men! e tu?",
		time: "April 25, 2018 21:05:11",
		status: 2,
		recvId: 0,
		recvIsGroup: false
	},
	{
		id: 4,
		sender: 0,
		body: "tá-se benne",
		time: "April 26, 2018 09:17:03",
		status: 1,
		recvId: 3,
		recvIsGroup: false
	},
	{
		id: 5,
		sender: 3,
		body: "alguém no discord?",
		time: "April 27, 2018 18:20:11",
		status: 0,
		recvId: 1,
		recvIsGroup: true
	},
	{
		id: 6,
		sender: 1,
		body: "viste aquela cena marada?",
		time: "April 27, 2018 17:23:01",
		status: 1,
		recvId: 0,
		recvIsGroup: false
	},
	{
		id: 7,
		sender: 0,
		body: "bora jogar cs? Matar uns Badegos?",
		time: "April 27, 2018 08:11:21",
		status: 2,
		recvId: 2,
		recvIsGroup: false
	},
	{
		id: 8,
		sender: 2,
		body: "népia, tenho trabalho pa acabar.. tu vais jogar com o diogo?",
		time: "April 27, 2018 08:22:12",
		status: 2,
		recvId: 0,
		recvIsGroup: false
	},
	{
		id: 9,
		sender: 0,
		body: "clároooooo",
		time: "April 27, 2018 08:31:23",
		status: 1,
		recvId: 2,
		recvIsGroup: false
	},
	{
		id: 10,
		sender: 0,
		body: "bora matar uns badegos no cs!! vou mandar msg ao alex!!!",
		time: "April 27, 2018 22:41:55",
		status: 2,
		recvId: 4,
		recvIsGroup: false
	},
	{
		id: 11,
		sender: 1,
		body: "5 min e tou lá",
		time: "April 28 2018 17:10:21",
		status: 0,
		recvId: 1,
		recvIsGroup: true
	}
];
*/

let MessageUtils = {
	getByGroupId: (groupId) => {
		return messages.filter(msg => msg.recvIsGroup && msg.recvId === groupId);
	},
	getByContactId: (contactId) => {
		return messages.filter(msg => {
			return !msg.recvIsGroup && ((msg.sender === user.id && msg.recvId === contactId) || (msg.sender === contactId && msg.recvId === user.id));
		});
	},
	getMessages: () => {
		return messages;
	},
	changeStatusById: (options) => {
		messages = messages.map((msg) => {
			if (options.isGroup) {
				if (msg.recvIsGroup && msg.recvId === options.id) msg.status = 2;
			} else {
				if (!msg.recvIsGroup && msg.sender === options.id && msg.recvId === user.id) msg.status = 2;
			}
			return msg;
		});
	},
	addMessage: (msg) => {
		msg.id = messages.length + 1;
		messages.push(msg);
	}
};
EXTERNAL changeCurrency(amount, index)
EXTERNAL changePersonalObjective(amount, index)
EXTERNAL changeScore (amonut, index)
EXTERNAL changeKills (amount, index)

EXTERNAL PlayEventAudio(index)

-> paragraph_1

=== paragraph_1 ===

“Im baaack!” from a puff of red smoke, your former boss has shown up once more!

“I'm sure I'm just imagining things, but are you guys actually heading towards my castle?” He lets out a scoff before continuing. “Surely you don't think you can beat me? There’s a reason that you guys are little employees, whilst I'm the boss!”

“But since I'm such a great boss I'm here to offer you a deal, you go back to being loyal and wimpy employees, and in return I won't kill you!”

What do you do?

* [Take the deal] -> take_deal

* [Counter offer!] -> counter_offer

* [Refuse] -> refuse

* [Punch him!] -> punch_king

=== take_deal ===
~ changeCurrency(5,0)
~ changeCurrency(5,1)
~ changeCurrency(5,2)
~ changeCurrency(5,3)
You tell him you accept the deal, if you get one month's pay up front!

“I guess that sounds fair… okey!” The king gives all of you <b><color=\#ffd966>+5 gold</color></b> each

5 gold? That's it? You guys earned way more whilst killing spirits! You tell the king that you don't accept such a low salary and are therefore quitting.

“You're always so ungrateful! Give me the gold back at least!” You tell him that its your gold now.

 He lets out an agitated groan before telling you that you're all gonna regret this, and once more disappearing in a puff of smoke. 

Look at that, this time you guys quit instead of being fired that gotta feel good! You all carry on with your journey, quite happy with your <b><color=\#ffd966>+5 free gold</color></b>!

-> END
=== counter_offer ===

You tell him that you have a counter offer; he can go and fudge himself! Oof!

You all laugh at your sick burn, the king, however, does not seem as amused by your antics. 

“See this is the problem with you guys, you can never take anything serious!” He stomps his foot into the ground and throws what seems to be a temper tantrum. Too preoccupied by his anger, he doesn't seem to notice all of the gold falling out of his pocket! Someone who notices however is <color=\#70e2fe> Cobalt</color>.

“Here I am offering you your dream jobs back and this is the thanks I get? You know what, offer REVOKED! Now I've decided to kill you again.” He flips all of you off before once more disappearing. 

As soon as the king has left, <color=\#70e2fe> Cobalt</color> dives onto the floor only to notice that the gold disappeared with the king.

You all gain <b><color=\#70e2fe>+1 very sad Cobalt:(</color></b>

-> END
=== refuse ===
~ changeCurrency(15,0)
~ changeCurrency(15,1)
~ changeCurrency(15,2)
~ changeCurrency(15,3)
You politely tell him you are still quite set on killing him and therefore have to refuse the deal.

“Wow, you guys have like integrity now? I respect it!” He pauses and seems to be looking at all of you from a new perspective, maybe you guys just got off on the wrong foot al those years ago, maybe all you needed was- 

“I'm still gonna kill you guys tho! Bye losers!" He once more disappears in a puff of red smoke. 

You all gain a new sense of integrity, which roughly translates to <b><color=\#ECBD00>+15 gold</color></b> each! Epic!

-> END
=== punch_king ===
~ changeCurrency(-20,0)
~ changeCurrency(-20,1)
~ changeCurrency(-20,2)
~ changeCurrency(-20,3)
You all decide to punch him!

As your fists are supposed to make an impact with the king, they instead seem to go straight through him instead!

“You guys don't think I would actually risk my life by actually going to talk to you? Not that I'm afraid of you or anything! Do I look like someone who would be afraid?” The king says this while looking very much like someone who would be afraid. 

“I see you guys aren't ready to communicate so i'm hereby revoking my deal! Now I've decided to kill you again! Bye losers!” He once more disappears in his puff of red smoke. 

You later notice that whilst trying to punch the king you accidentally dropped some gold, you all lose <b><color=\#e06666>-10 gold</color></b> each.

-> END
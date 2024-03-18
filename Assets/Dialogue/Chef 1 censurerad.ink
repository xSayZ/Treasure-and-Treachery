EXTERNAL changeCurrency(amount, index)
EXTERNAL changePersonalObjective(amount, index)
EXTERNAL changeScore (amonut, index)
EXTERNAL changeKills (amount, index)

EXTERNAL PlayEventAudio(index)

-> paragraph_1

=== paragraph_1 ===
~ PlayEventAudio(0)
“Well hello employees!” from a puff of red smoke, your boss shows up!

It seems like the poster you found earlier was telling the truth, unlike you guys who turned into monsters, the only thing different with Mr. Red is that he is now a king!

“Ew, you guys are like super ugly! I mean, you weren't all that pretty before, but now you're hideous!” He gives each one of you a once over, seemingly more disgusted for each one of you he looks at. 

“I quite like this kingdom of mine, but this world seems to have one major flaw…”

“YOU.” 

“I really would rather my new paradise not be infected by my old annoying employees, so if you take so much as a step closer to my magnificent castle, I have decided to end you! You understand don't you?” 

What do you do?

* [Stick it to the man!] -> stick_it

* [Tell him you actually don't understand] -> dont_understand

* ["No you!"] -> no_you

* [Accept your fate] -> accept_fate

===stick_it===
~ changeCurrency(10,0)
~ changeCurrency(10,1)
~ changeCurrency(10,2)
~ changeCurrency(10,3)
He ain't the boss of you! I mean he literally was a couple of hours ago, but you guys died so now he isnt! 

He can't push you around anymore, and to prove this you all decide to push him! with a satisfying “omph!” The newly crowned king lands straight on his ass.

“You're all gonna regret this!” he says before vanishing in a new puff of smoke. It would probably seem a lot more intimidating if he wasn't still sitting on the ground as he disappeared. 

You all gain <b><color=\#ECBD00>+10 gold</color></b>

-> END
===dont_understand===
~ changeCurrency(-15, 0)
~ changeCurrency(-15, 1)
~ changeCurrency(-15, 2)
~ changeCurrency(-15, 3)
You explain to him that you didn't fully understand and would like for him to repeat himself. 

”Oh okey!” He says before he repeats his whole speech from the beginning. It’s not until he has once more finished that he looks up and sees the grin on all of your faces.

“Oh you think it’s sooo fun to mock me?” He says before shooting some kind of red magic on you guys!

“See if it’s still fun when you guys, not only get murdered by my minions, but also die poor!” He yells and disappears in a new cloud of smoke. 

You all feel your pockets empty, and notice that you seem to have lost <b><color=\#D30000>-15 gold</color></b> each! 

You all spend the next thirty minutes trying to comfort the now hysteric Cobalt...

-> END
===no_you===
~ changeCurrency(-15, 0)
~ changeCurrency(-15, 1)
~ changeCurrency(-15, 2)
~ changeCurrency(-15, 3)
You see him physically recoil from the impact of your words! 

“Take that back!” He’s got that look in his face he always gets when someone forgets to refill the ink in the copy machine. Pure anger.

He repeatedly tells you to take it back, but eventually accepts your unwillingness to. 

“Suit yourselves!” He says before dejectedly disappearing in a new puff of red smoke.

By winning this battle of wits you feel yourself somehow gain<b><color=\#ECBD00>+15 gold</color></b> each! Good jobb!

-> END
===accept_fate===
~ changeCurrency(5,0)
~ changeCurrency(5,1)
~ changeCurrency(5,2)
~ changeCurrency(5,3)
You tell him that you don’t really mind him trying to kill you, if you were in his situation you would probably do the same.

“Wow, I didn't think you guys would be so understanding! You know what , I'll go a little easier on you guys!” His earlier frown turns into a little smile and he gives you all an, slightly awkward, wave before he once more disappears in a new puff of smoke!

From accepting your fate you all seem to have gained <b><color=\#ECBD00>+5 gold</color></b> each! Epic!

Seems like the first step really is acceptance, the first step to kicking the king's butt that is!

-> END
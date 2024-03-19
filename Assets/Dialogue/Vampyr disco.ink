EXTERNAL changeCurrency(amount, index)
EXTERNAL changePersonalObjective(amount, index)
EXTERNAL changeScore (amonut, index)
EXTERNAL changeKills (amount, index)

EXTERNAL PlayEventAudio(index)

-> paragraph_1

=== paragraph_1 ===
Amidst the ruins you come across a vampire disco, how exciting! But the party seems to be kind of dead at the moment. The vampires are just kind of sitting or hanging around the ruins, sipping on a red liquid and having low conversations. 

What do you do?

* [Suggest a blood sacrifice!] -> Blood_sacrifice
* [Show them your sick party moves!] -> Party_moves
* [Make <color=\#ed9177> Rusty</color> howl his self composed song!] ->Howl_song
* [Enjoy the chill vibe!] -> Chill

=== Blood_sacrifice===
~ changeScore (-10,0)
~ changeScore (-10,1)
~ changeScore (-10,2)
~ changeScore (-10,3)
You guys know exactly what would liven up this lame party, a good old fashioned blood sacrifice! 

Blood and sacrifice? Vampires should love this! 

Firstly you just need to pick a victim. You randomly point out one vampire to be sacrificed and watch as everyone’s faces quickly turn sour!

Oh no! It turns out you’ve accidentally chosen Gary, and everyone <b>loves Gary!</b>

It quickly turns into a physical confrontation. You all put up a good fight but there just seems to be too many vampires for you guys. After a long battle you all make it out of the fight with slightly wounded health, but greatly wounded pride.

Your shame makes you all lose <b><color=\#e06666> -10 total score</color></b> each. -> END

===Party_moves===
~ changeCurrency(5, 0)
~ changeCurrency(5, 1)
~ changeCurrency(5, 2)
~ changeCurrency(5, 3)
~ changeScore (20,0)
~ changeScore (20,1)
~ changeScore (20,2)
~ changeScore (20,3)

You all unleash your inner party animals! <color=\#70e2fe> Cobalt</color> quickly does a keg stand, Violet uses their magic to put on a firework show, and both <color=\#ed9177> Rusty</color> and Jade whip out some epic dance moves!

You all greatly impress the undead partygoers, inspiring them to join you making the party turn a lot more rowdy! 

The party seems to go on all night and its not until the sun has almost come up that the vampires decide to take their leave. Before going they all reward you with gold as a thank you for saving their lame party.

You all gain <b><color=\#ECBD00>+5 gold</color></b> and +20 total score -> END

===Howl_song===
~ changeCurrency(2,0 )
~ changeCurrency(18,1)
With a little convincing by the rest of the group, <color=\#ed9177> Rusty</color> agrees to howl his self composed song!

The night gets filled with <color=\#ed9177> Rusty</color>s wolfish wails. It’s a moving piece about love, loss, and the bravery of being yourself in spite of the people trying to tear you down.

Once the song has finished you see many of the vamps wiping their tear stained cheeks. <color=\#70e2fe> Cobalt</color>, under the guise of being <color=\#ed9177> Rusty</color>s agent, takes the opportunity to gather tips from the awestruck audience.

<color=\#70e2fe> Cobalt</color> makes sure to later give <color=\#ed9177> Rusty</color> his fair share of the profit, with a <i> generous </i> 10/90 deal.

<color=\#ed9177> Rusty</color> gains  <b><color=\#ffd966>+2 Gold</color></b> and <color=\#70e2fe> Cobalt</color> gains  <b><color=\#ffd966>+18 Gold</color></b>, both of them seemingly just as happy with their earnings!


 -> END

===Chill===
~changePersonalObjective (-20, 0)

Ain't nothing wrong with a calm little gathering! As a matter of fact, it might be nice with a little intensity shift after all the battles and whatnot you guys have gone through lately. 

You all sit down and have meaningful and deep conversations with the vampires. Turns out you guys have a lot in common and you all help each other come to important insight about yourselves, and each other!

Even though the vibes are great you decide to leave the gathering early, there's a day tomorrow as well after all. 

You all have a great evening and earn <b><color=\#8F00FF>+1 feeling of yourselfs turning into your parents…</color></b> <i> shudders <i>

The evening seems to have been so relaxing that <color=\#ed9177> Rusty</color>'s lost about <b><color=\#ed9177>-20 wrath,</color></b> which is probably really good for his heart but is kinda lame for being a raging werewolf…

-> END


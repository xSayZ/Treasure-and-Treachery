EXTERNAL changeCurrency(amount, index)
EXTERNAL changePersonalObjective(amount, index)
EXTERNAL changeScore (amonut, index)
EXTERNAL changeKills (amount, index)

EXTERNAL PlayEventAudio(index)


-> paragraph_1

=== paragraph_1 ===

Whilst riding in the carriage you suddenly hear a loud meowing sound coming from somewhere off the side of the road. 

The carriage slows down and you are all able to make out that the sad cries appear to be coming from a little spirit in the form of a cat. The poor cat seems to have gotten itself stuck in a tree and is now in dire need of a kind soul to help it down.  

    What do you do?

    * [Climb up the tree and help the cat down] -> cat_helped

    * [Punch the tree until the cat falls down] -> punch_tree
    
    * [Shoot the cat] -> shooting
    
    * [Communicate with the cat] -> communicate

=== cat_helped ===
~ changeCurrency(-5, 0)
~ changeCurrency(-5, 1)
~ changeCurrency(-5, 2)
You all collaborate to climb the tree and bring the cat down. With a little convincing, <color=\#ed9177>Rusty</color> and <color=\#ac70fe>Violet</color> make a human tower, allowing <color=\#70e2fe>Cobalt</color> to use them as a makeshift ladder to climb the tree. <color=\#96d87d>Jade</color> stands off to the side cheering the group on, slightly upset that she doesn't have a larger part in the rescue mission. 

The plan goes perfect until it's time for <color=\#70e2fe>Cobalt</color> to grab the cat. The cat, terrified by the blue scaly creature, scratches <color=\#70e2fe>Cobalt</color> which makes them fall. <color=\#70e2fe>Cobalt</color> lands on top of <color=\#ac70fe>Violet</color> which then results in them both falling on top of <color=\#ed9177>Rusty</color>. 

    Positives? <color=\#96d87d> Jade</color> is no longer jealous that she wasn't a part of the rescue mission!<br> 
    
    Negatives? <color=\#70e2fe> Cobalt</color>, <color=\#ac70fe> Violet</color> and <color=\#ed9177> Rusty</color> all lose <b><color=\#ffd966>-3 gold</color></b> in the fall -> END

=== punch_tree ===
~ changePersonalObjective (20, 0)
Really, this is your plan? Sure, why not. You all move to stand on different sides of the tree and then start to simultaneously punch it. The tree is hard and its bark scratches you with each hit. 

You quickly realize that punching trees really isn't as fun as one would like to think, well, unless your name is <color=\#ed9177> Rusty</color>. 

<color=\#ed9177> Rusty</color> is going fucking in on that tree! Honestly this might be really good for him, a safe way to let out his anger! …Or it's completely useless, but hey, at least the guy is having fun.

By some miracle your strategy seems to actually work and the cat eventually falls down from the tree, landing safely on its feet.

“Thank you for helping me down, guys!”. Says the cat, who apparently speaks English, before quickly scurrying away. 

Whilst punching the tree <color=\#ed9177> Rusty</color> seems to have worked up quite a bit of rage, <b><color=\#FC6A03> +20 Rage</color></b> to be specific! Epic!

 -> END


=== shooting ===
~ changePersonalObjective (5, 2)
Really, you decided to shoot the cat? What are you guys, monsters?

Oh, right, you are!

The cat gets completely disintegrated and disappears in a puff of smoke, only leaving behind some spirit dust. 

Yucky!

The party gets <b><color=\#fc8ff8> +1 dead ghost cat on everyone's conscience!</color></b>

…which doesnt mean a lot for most of you but counts as  <b><color=\#ac70fe> +5 souls</color></b> for <color=\#ac70fe> Violet</color>!
Neat!-> END

=== communicate ===
For some reason you all decide that the best option is to communicate with the cat in the universal language of song. Just to make sure the ghostly cat will truly understand you also decide to sing it in meows. Brilliant idea…

You all start meowing a surprisingly beautiful quartet. All those office party performances are actually paying off! The cat's sad meows eventually quiet down only to come back louder, but happier, as it joins in on the song. 

Encouraged by your song the cat is now brave enough to jump down on its own. 

“That was great guys! Here take my business card, in case you ever wanna make a career out of this!” 

The cat hands you all tiny business cards and then scurries away. It’s only after the cat has left that the party realizes it had just spoken in perfect English.

 You all gain <b><color=\#fc8ff8>+1 Cat business card!</color></b> -> END







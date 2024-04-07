<Cabbage>
form caption("Untitled") size(400, 700), guiMode("queue"), pluginId("def1")
keyboard bounds(8, 196, 381, 95)
hslider bounds(22, 82, 150, 50) channel("rvbamt") range(0, 1, 0, 1, 0.001) text("rvb amt") popupText("rvb amt")
hslider bounds(14, 142, 150, 50) channel("freq") range(0, 20000, 55, 1, 0.001) text("freq") popupText("freq")
hslider bounds(204, 90, 150, 50) channel("delamt") range(0, 1, 0, 1, 0.001) text("del amt") popupText("del amt")
hslider bounds(208, 310, 150, 50) channel("pitchbend") range(-5, 5, 0, 1, 0.001) text("pitchbend") popupText("pitchbend")
hslider bounds(14, 308, 150, 50) channel("distance") range(0, 1, 0, 1, 0.001) text("distance") popupText("distance")
hslider bounds(204, 24, 150, 50) channel("lfoamp") range(0, 5, 0, 1, 0.001) text("lfoamp") popupText("lfoamp")
hslider bounds(20, 24, 150, 50) channel("lforate") range(0, 5, 0, 1, 0.001) text("lfo rate") popupText("lfo rate")


checkbox bounds(234, 150, 100, 30) channel("noteon")
</Cabbage>
<CsoundSynthesizer>
<CsOptions>
-n -d -+rtmidi=NULL -M0 --midi-key-cps=4 --midi-velocity-amp=5
</CsOptions>
<CsInstruments>
; Initialize the global variables. 
ksmps = 32
nchnls = 2
0dbfs = 1


; initialize zak space
zakinit 3, 1

instr 22
    ; custom adsr based on chnget
    ktrigger init 0
    knoteon init 0
    knoteon = chnget:k("noteon")
    ktrigger = changed2:k(knoteon)
    kvol = 1 ;change for velocity input
    kamp init 0
    

    if ktrigger == 1 then
        if knoteon == 1 then
            event "i", 1, 0, -1 
        else
            event "i", -1, 0, 1
        endif
        ktrigger = 0
    endif
endin

//instrument will be triggered by keyboard widget
instr 1

    // Unity Inputs
    kdistance = chnget:k("distance")

    ; cps taken from chnget
    kfreq init 50
    kfreq = lag(chnget:k("freq"),0.5)
   
  ; From: oscilikt csound manual
  ; LFO
  kamp1 = 1*kdistance//chnget:k("lfoamp")
  kcps1 = 10*kdistance//chnget:k("lforate")
  itype = 0
  ksine lfo kamp1, kcps1, itype
  
  // envelope 
  kamp = madsr:k(0.1, 0.1, 0.8, 0.4)
  kpitchbend = chnget:k("pitchbend")
  
  ;random
  //krandpitch = randomi:k(1, 5, 1, 2, 3)
  
  // WGBOW -- SOUND GENERATOR
    kpres = 4							;pressure value
    krat = 0.127236						;position along string
    kvibf = 6.12723
		
    kvamp = kamp * 0.01
    aOutR  wgbow kamp*0.5, kfreq + ksine*kdistance, kpres, kpres, kvibf, kvamp, 1
    aOutL  wgbow kamp*0.5, kfreq + ksine*kdistance, kpres, kpres+0.5, kvibf, kvamp, 1
    
    //distortion
    aOutR powershape aOutR, 1.2-kdistance
    aOutL powershape aOutL, 1.2-kdistance
    
    //filter
    aOutR zdf_ladder aOutR, 20000*kdistance, 5
    aOutL zdf_ladder aOutL, 20000*kdistance, 5
    
  //verb send
  krvbAmt = 0.3//chnget:k("rvbamt")
  zawm aOutL*krvbAmt, 1
  zawm aOutR*krvbAmt, 2
  
  //delay send
  kdelAmt = chnget:k("delamt")
  aMono = (aOutL+aOutR) * 0.5
  zawm aMono, 3
outs aOutL, aOutR
endin

//--------------------------------------------------------
//REVERB
instr 5 ; From: https://flossmanual.csound.com/sound-modification/reverberation
aInSigL       zar       1    ; read first zak audio channel
aInSigR       zar       2
denorm(aInSigL)
denorm(aInSigR)
kFblvl       init      0.77 ; feedback level - i.e. reverb time
kFco         init      8000 ; cutoff freq. of a filter within the reverb
; create reverberated version of input signal (note stereo input and output)
aRvbL,aRvbR  reverbsc  aInSigL, aInSigR, kFblvl, kFco
             outs      aRvbL, aRvbR ; send audio to outputs
             zacl      0, 2        ; clear zak audio channels
endin

//--------------------------------------------------------------------

;instr 6 ;delay DISABLED
;aInSig       zar       3    ; read second zak audio channel
;kdelAmt = chnget:k("delamt")
;asig vdelay3 aInSig, 1000*kdelAmt, 10000
;outs asig, asig
;zacl      3, 3         ; clear zak audio channels
;zawm asig*0.5, 2 //feedback
;endin
</CsInstruments>
<CsScore>
 //Table #1, a sine waveform.
f 1 0 4096 10 1 0 0 0.22 0 0.1 0.5 0 0.2
 //Table #2: a sawtooth wave
f 2 0 3 -2 1 0 -1
;causes Csound to run for about 7000 years...
f0 z
i 22 0 z
i 5 0 z
//i 6 0 z
</CsScore>
</CsoundSynthesizer>

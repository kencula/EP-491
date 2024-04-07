<Cabbage>
form caption("Untitled") size(400, 300), guiMode("queue"), pluginId("def1")
keyboard bounds(8, 196, 381, 95)
hslider bounds(20, 90, 150, 50) channel("rvbamt") range(0, 1, 0, 1, 0.001) text("rvb amt") popupText("rvb amt")
hslider bounds(204, 90, 150, 50) channel("delamt") range(0, 1, 0, 1, 0.001) text("del amt") popupText("del amt")
hslider bounds(20, 150, 150, 50) channel("pitchbend") range(-5, 5, 0, 1, 0.001) text("rvb amt") popupText("rvb amt")
hslider bounds(204, 24, 150, 50) channel("lfoamp") range(0, 5, 0, 1, 0.001) text("lfoamp") popupText("lfoamp")
hslider bounds(20, 24, 150, 50) channel("lforate") range(0, 5, 0, 1, 0.001) text("lfo rate") popupText("lfo rate")
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


; initialize zak space  - one a-rate and one k-rate variable.
zakinit 2, 1

//instrument will be triggered by keyboard widget
instr 1

  ; From: oscilikt csound manual
  ; Generate a uni-polar (0-1) square wave.
  kamp1 = chnget:k("lfoamp")
  kcps1 =chnget:k("lforate")
  itype = 0
  ksine lfo kamp1, kcps1, itype

  kpitchbend = chnget:k("pitchbend")
  aOut oscilikt p5, (p4+ksine)*(1+kpitchbend*0.01), 1
  
  //verb send
  krvbAmt = chnget:k("rvbamt")
  zawm aOut*krvbAmt, 1
  
  //delay send
  kdelAmt = chnget:k("delamt")
  zawm aOut*kdelAmt, 2
outs aOut, aOut
endin

instr 5 ; From: https://flossmanual.csound.com/sound-modification/reverberation
aInSig       zar       1    ; read first zak audio channel
denorm(aInSig)
kFblvl       init      0.88 ; feedback level - i.e. reverb time
kFco         init      8000 ; cutoff freq. of a filter within the reverb
; create reverberated version of input signal (note stereo input and output)
aRvbL,aRvbR  reverbsc  aInSig, aInSig, kFblvl, kFco
             outs      aRvbL, aRvbR ; send audio to outputs
             zacl      0, 1         ; clear zak audio channels
endin

instr 6 ;delay
aInSig       zar       2    ; read second zak audio channel
kdelAmt = chnget:k("delamt")
asig vdelay3 aInSig, 1000*kdelAmt, 10000
outs asig, asig
zacl      2, 2         ; clear zak audio channels
zawm asig*kdelAmt, 2
endin
</CsInstruments>
<CsScore>
 //Table #1, a sine waveform.
f 1 0 4096 10 0 1
 //Table #2: a sawtooth wave
f 2 0 3 -2 1 0 -1
;causes Csound to run for about 7000 years...
f0 z
i 5 0 z
i 6 0 z
</CsScore>
</CsoundSynthesizer>
